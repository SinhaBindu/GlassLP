using GlassLP.Data;
using GlassLP.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace GlassLP.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(string email, string userName, string password)
        {
            var user = new ApplicationUser { UserName = userName, Email = email };
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(string username, string password, bool isPersistent = false)
        {
            return await _signInManager.PasswordSignInAsync(username, password, isPersistent, lockoutOnFailure: false);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser?> FindUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            }
            return user;
        }

        public async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task SignInUserAsync(ApplicationUser user, bool isPersistent = false)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }
    }
}

