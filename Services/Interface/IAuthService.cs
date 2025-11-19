using GlassLP.Data;
using GlassLP.DTO;
using Microsoft.AspNetCore.Identity;

namespace GlassLP.Services.Interface
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(string email, string userName, string password);
        Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(string username, string password, bool isPersistent = false);
        Task SignOutAsync();
        Task<ApplicationUser?> FindUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<bool> ValidatePasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task SignInUserAsync(ApplicationUser user, bool isPersistent = false);
    }
}

