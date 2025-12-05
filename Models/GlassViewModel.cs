using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassLP.Models
{
    public class GlassViewModel
    {
        public int pk_Glassid { get; set; } = 0;

        [DisplayName("Type of Model")]
        public int TypeOfModuleId { get; set; }
        [DisplayName("Receive Date")]
        public DateTime? ReceiveDate { get; set; }

        [DisplayName("Power of Glass")]
        public int PowerOfGlassId { get; set; }

        [DisplayName("No. of Glasses")]
        public int NoofGlasses { get; set; }

        [DisplayName("Per Glass Cost")]
        [Range(0, 999999.99, ErrorMessage = "Per Glass Cost must be between 0 and 999999.99")]
        public decimal? PerGlassCost { get; set; } = 0;

        [DisplayName("Total Glass Cost")]
        [Range(0, 999999.99, ErrorMessage = "Total Glass Cost must be between 0 and 999999.99")]
        public decimal? TotalGlassCost { get; set; } = 0;

        [DisplayName("Advertising Cost")]
        [Range(0, 999999.99, ErrorMessage = "Advertising Cost must be between 0 and 999999.99")]
        public decimal? AdvertisingCost { get; set; } = 0; // M2

        [DisplayName("Storage/Miscellaneous Cost")]
        [Range(0, 999999.99, ErrorMessage = "Storage/Miscellaneous Cost must be between 0 and 999999.99")]
        public decimal? Storagemiscellaneouscost { get; set; } = 0; // M2

        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> TypeOfModuleList { get; set; }

        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> PowerOfGlassList { get; set; }

        [NotMapped]
        public string? TypeOfModuleName { get; set; }

        [NotMapped]
        public string? PowerOfGlassName { get; set; }

        [NotMapped]
        [ValidateNever]
        public string? Version { get; set; }

        [NotMapped]
        [ValidateNever]
        public DateTime? SynDate { get; set; }

        [NotMapped]
        [ValidateNever]
        public string? uuid { get; set; }

        [NotMapped]
        [ValidateNever]
        [DisplayName("Available Glass in Stock")]
        public int? Availableclassesinstock { get; set; }

        [NotMapped]
        [ValidateNever]
        public int? TotalDistributedGlass { get; set; }
    }
}
