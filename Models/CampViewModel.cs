using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlassLP.Models
{
    public class CampViewModel
    {
        // Form fields
        public int CampId_pk { get; set; } = 0;
        public int? TypeOfModule { get; set; }
        public int? TypeOfVisit { get; set; }
        public string? CampCode { get; set; }
        public int DistrictId { get; set; }
        public int? BlockId { get; set; }
        public int? CLFId { get; set; }
        public int? PanchayatId { get; set; }
        public string? VOName { get; set; }
        public DateTime? CampDate { get; set; }
        public string? Location { get; set; }
        public string? CRPName { get; set; }
        public string? CRPMobileNo { get; set; }
        public int? ParticipantMobilized { get; set; }
        public int? TotalScreened { get; set; }
        public int? TotalGlassesDistributed { get; set; }
        public int? PowerOfGlassId { get; set; }
        public IFormFile? PhotoUpload { get; set; }
        public string? PhotoUploadPath { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // ...other properties...

        // Dropdown sources
        public IEnumerable<SelectListItem> TypeOfModuleList { get; set; }
        public IEnumerable<SelectListItem> TypeOfVisitList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> BlockList { get; set; }
        // etc.
    }
}
