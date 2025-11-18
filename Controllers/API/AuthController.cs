using GlassLP.Data;
using GlassLP.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using GlassLP.Utilities;

namespace GlassLP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTHelper _jwtHelper;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, Utilities.JWTHelper jwtHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Validation failed",
                    reason = "Invalid model state",
                    data = (object)null
                });
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                
                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);
                // --- JWT Token Generate ---
                var token = _jwtHelper.GenerateJwtToken(user.Id, user.UserName,roles.FirstOrDefault());

                return Ok(new
                {
                    status = true,
                    message = "Login successful",
                    reason = "OK",
                    data = new
                    {
                        token = token,
                        user = new
                        {
                            UserId = user.Id,
                            Name = user.Name,
                            Role = roles.FirstOrDefault()
                        }
                    }
                });
            }

            return Unauthorized(new
            {
                status = false,
                message = "Login failed",
                reason = "Invalid login attempt",
                data = (object)null
            });
        }

    }




}
