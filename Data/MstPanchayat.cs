using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassLP.Data
{
    [Table("mst_Panchayat")]
    public class MstPanchayat
    {
        [Key]
        public int PanchayatId_pk { get; set; }

        public int? DistrictId_fk { get; set; }
        public int? BlockId_fk { get; set; }

        [StringLength(500)]
        public string PanchayatName { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(130)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
