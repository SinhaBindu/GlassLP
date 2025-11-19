using GlassLP.Data;
using GlassLP.DTO;
using GlassLP.Models;
using GlassLP.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GlassLP.Controllers.API
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTHelper _jwtHelper;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JWTHelper jwtHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(entry => entry.Value?.Errors.Count > 0)
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "validation_failed",
                    "Please provide both username and password.",
                    new List<object> { validationErrors }));
            }

            var user = await _userManager.FindByNameAsync(loginDto.UserName)
                       ?? await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null)
            {
                return Unauthorized(new ApiResponse<List<object>>(
                    false,
                    "invalid_credentials",
                    "Username or password is incorrect.",
                    new List<object>()));
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Unauthorized(new ApiResponse<List<object>>(
                    false,
                    "invalid_credentials",
                    "Username or password is incorrect.",
                    new List<object>()));
            }

            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? string.Empty;
            var token = _jwtHelper.GenerateJwtToken(user.Id, user.UserName ?? string.Empty, primaryRole);

            var responsePayload = new List<object>
            {
                new
                {
                    token,
                    user = new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        email = user.Email,
                        roles
                    }
                }
            };

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                responsePayload));
        }
    }
}

