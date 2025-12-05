using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GlassLP.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Name")]
        public string? Name { get; set; }

        [DisplayName("Password")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        [DisplayName("District")]
        public string? DistrictId { get; set; }

        [DisplayName("Block")]
        public int? BlockId { get; set; }

        [DisplayName("Panchayat")]
        public int? PanchayatId { get; set; }

        [DisplayName("CLF")]
        public int? CLFId { get; set; }

        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Email Confirmed")]
        public bool EmailConfirmed { get; set; } = true;

        // Dropdown lists
        public IEnumerable<SelectListItem>? RoleList { get; set; }
        public IEnumerable<SelectListItem>? DistrictList { get; set; }
        public IEnumerable<SelectListItem>? BlockList { get; set; }
        public IEnumerable<SelectListItem>? PanchayatList { get; set; }
        public IEnumerable<SelectListItem>? CLFList { get; set; }

        // Display only fields
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

		public int MapId { get; set; }
		public List<int>? DistrictIds { get; set; }

		public string? RoleId { get; set; }
	}
    public class UserListVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
		public string RoleId { get; set; }
		public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
		public string DistrictIds { get; set; }
		public string BlockId { get; set; }
        public string CLFId { get; set; }
        public string DistrictName { get; set; }
        public string BlockName { get; set; }
        public string CLFName { get; set; }
        public string PanchayatName { get; set; }
       
    }
}

