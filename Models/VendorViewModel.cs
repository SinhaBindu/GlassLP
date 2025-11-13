using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace GlassLP.Models
{
    public class VendorViewModel
    {
        public int Vendors_pk { get; set; } = 0;

        [DisplayName("Business Mentor Name")]
        public string? BusinessMentorName{ get; set; }

        [DisplayName("VE Name")]
        public string? VEName { get; set; }

        [DisplayName("CLF Name")]
        public int CLFId { get; set; }

        [DisplayName("AOP Name")]
        public string? AOPName { get; set; }

        [DisplayName("Contact Number")]
        public string? ContactNumber { get; set; }

        [DisplayName("Total Sales Amount")]
        public string? TotalSalesAmount{ get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
