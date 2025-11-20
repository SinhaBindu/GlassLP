using Microsoft.AspNetCore.Identity;

namespace GlassLP.Data
{
    public class ApplicationUser : IdentityUser
    {
        // add custom properties if needed
        // public string DisplayName { get; set; }
        public int? DistrictId { get; set; }
        public int? BlockId { get; set; }
        public int? PanchayatId { get; set; }
        public int? CLFId { get; set; }
        public string? Name { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }

    }
}
