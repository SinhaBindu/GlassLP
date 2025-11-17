using Microsoft.AspNetCore.Identity;

namespace GlassLP.Data
{
    public class ApplicationUser : IdentityUser
    {
        // add custom properties if needed
        // public string DisplayName { get; set; }
        public int? DistrictId { get; set; }
        public int? BlockId { get; set; }
        public string? Name { get; set; }
    }
}
