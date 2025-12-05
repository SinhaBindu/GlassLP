using GlassLP.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using static GlassLP.Data.Service;

namespace GlassLP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly SPManager _spManager;
		private readonly GlobalDataService _globalData;
		public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager, SPManager spManager,GlobalDataService globalData)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _spManager = spManager;
			_globalData = globalData;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string email, string userName, string password)
        {
            if (!ModelState.IsValid) return View();

            var user = new ApplicationUser { UserName = userName, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // optionally add default role, or sign-in directly
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var err in result.Errors) ModelState.AddModelError("", err.Description);
            return View();
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);
				var user = await _userManager.FindByNameAsync(username);
				if (user != null)
				{
					// Populate GlobalDataService
					var globalData = _spManager.GetLoggedInUser(user.Id);

					//// Optional: store in DI service for later use
					//_globalData.UserId = globalData.UserId;
					//_globalData.UserName = globalData.UserName;
					//_globalData.PhoneNumber = globalData.PhoneNumber;
					//_globalData.Email = globalData.Email;
					//_globalData.RoleId = globalData.RoleId;
					//_globalData.Role = globalData.Role;
					//_globalData.DistrictIds = globalData.DistrictIds;
					//_globalData.DistrictName = globalData.DistrictName;
					//_globalData.BlockId = globalData.BlockId;
					//_globalData.BlockName = globalData.BlockName;
					//_globalData.CLFId = globalData.CLFId;
					//_globalData.CLFName = globalData.CLFName;
					//_globalData.LoginTime = globalData.LoginTime;

					var claims = new List<Claim>
                    {
	                    new Claim("UserId", globalData.UserId),
	                    new Claim("UserName", globalData.UserName),
	                    new Claim("PhoneNumber", globalData.PhoneNumber),
	                    new Claim("Email", globalData.Email),
	                    new Claim("RoleId", globalData.RoleId),
	                    new Claim("DistrictIds", globalData.DistrictIds),
	                    new Claim("DistrictName", globalData.DistrictName),
	                    new Claim("BlockId", globalData.BlockId),
	                    new Claim("BlockName", globalData.BlockName),
	                    new Claim("CLFId", globalData.CLFId),
	                    new Claim("CLFName", globalData.CLFName),
	                    new Claim("LoginTime", globalData.LoginTime)
                    };

					var identity = new ClaimsIdentity(claims, "login");
					await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
				}

				return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [HttpGet]
        public IActionResult Logout() => View();
        [HttpPost]
        public async Task<IActionResult> Logout1()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
