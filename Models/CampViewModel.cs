using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassLP.Models
{
    public class CampViewModel
    {
        // Form fields
        public int CampId_pk { get; set; } = 0;

        [DisplayName("Type of Module")]
        public int? TypeOfModule { get; set; }

        [DisplayName("Type of Visit")]
        public int? TypeOfVisit { get; set; }

        [DisplayName("Camp Code")]
        public string? CampCode { get; set; }

        [DisplayName("District")]
        [Required]
        public int? DistrictId { get; set; }

        [DisplayName("Block")]
        public int? BlockId { get; set; }

        [DisplayName("CLF Name")]
        public int? CLFId { get; set; }

        [DisplayName("Panchayat")]
        public int? PanchayatId { get; set; }

        [DisplayName("VO Name")]
        public string? VOName { get; set; }

        [DisplayName("Camp Date")]
        public DateTime? CampDate { get; set; }

        [DisplayName("Camp Location")]
        public string? Location { get; set; }
        //[Required]
        [DisplayName("CRP Name")]
        public string? CRPName { get; set; }
        [DisplayName("CRP Mobile No")]
        //[Required]
        public string? CRPMobileNo { get; set; }
        [DisplayName("Participants Mobilized")]
        public int? ParticipantMobilized { get; set; }
        [DisplayName("Total Screened")]
        public int? TotalScreened { get; set; }

        [DisplayName("Total Glasses Distributed")]
        public int? TotalGlassesDistributed { get; set; }
        public int? PowerOfGlassId { get; set; }
        [NotMapped]
        [ValidateNever]
        [DisplayName("Camp Image")]
        public IFormFile? PhotoUpload { get; set; }
        [NotMapped]
        [ValidateNever]
        [DisplayName("Camp Image")]
        public string PhotoUploadBase64 { get; set; }
        public string? PhotoUploadPath { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
		// ...other properties...
		//public string? DistrictName { get; set; }
		//[NotMapped]
		//public string? BlockName { get; set; }
		//[NotMapped]
		//public string? PanchayatName { get; set; }
		[DisplayName("Vision Entrepreneur")]
		public int? VEId { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Version { get; set; }
        [NotMapped]
        [ValidateNever]
        public DateTime? SynDate { get; set; }
        [NotMapped]
        [ValidateNever]
        public string uuid { get; set; }

        [NotMapped]
		[ValidateNever]
		public IEnumerable<SelectListItem> TypeOfModuleList { get; set; }
        //[NotMapped]
        //[ValidateNever]
        //public IEnumerable<SelectListItem> VEList { get; set; }

  //      [ValidateNever]
		//public IEnumerable<SelectListItem> TypeOfVisitList { get; set; }

		//[ValidateNever]
		//public IEnumerable<SelectListItem> DistrictList { get; set; }

		//[ValidateNever]
		//public IEnumerable<SelectListItem> BlockList { get; set; }
    }
}
