using System.ComponentModel.DataAnnotations;

namespace GlassLP.Models
{
    public class Filtermodel
    {
        [Display (Name ="District")]
        public int? DistrictId { get; set; }
		[Display(Name = "Block")]
		public int? BlockId { get; set; }
		[Display(Name = "CLF")]
		public int? CLFId { get; set; }
		[Display(Name = "Panchayat")]
		public int? PanchayatId { get; set; }
		[Display(Name = "Type Of Module")]
		public int? TypeOfModuleId { get; set; }
		[Display(Name = "Power Of Glass")]
		public int? PowerOfGlassId { get; set; }
		[Display(Name = "Vision Issue Id")]
		public int? VisionIssueId { get; set; }
		[Display(Name = "Occupation")]
		public int? OccupationId { get; set; }
		[Display(Name = "Camp Code")]
		public string? CampCode { get; set; }
    }
}
