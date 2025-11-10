
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassLP.Data
{
    // -------------------------
    // Entity classes
    // -------------------------

    [Table("mst_Block")]
    public class MstBlock
    {
        [Key]
        public int BlockId_pk { get; set; }
        public int? DistrictId_fk { get; set; }
        [StringLength(500)]
        public string BlockName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(130)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_CLF")]
    public class MstCLF
    {
        [Key]
        public int pk_CLFId { get; set; }

        [StringLength(100)]
        public string CLFName { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_District")]
    public class MstDistrict
    {
        [Key]
        public int DistrictId_pk { get; set; }

        [StringLength(500)]
        public string DistrictName { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(130)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_Federation")]
    public class MstFederation
    {
        [Key]
        public int FederationId_pk { get; set; }

        public int? DistrictId_fk { get; set; }
        public int? BlockId_fk { get; set; }
        public int? PanchayatId_fk { get; set; }

        [StringLength(500)]
        public string FederationName { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(130)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_Glass")]
    public class MstGlass
    {
        [Key]
        public int pk_Glassid { get; set; }
        public int? PowerOfGlass { get; set; }

        [StringLength(100)]
        public string GlassName { get; set; }

        public bool? IsActive { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_Occupation")]
    public class MstOccupation
    {
        [Key]
        public int pk_OccupationId { get; set; }

        [StringLength(100)]
        public string OccupatioName { get; set; }

        public bool? IsActive { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }



    [Table("tbl_Camp")]
    public class TblCamp
    {
        [Key]
        public int CampId_pk { get; set; }

        public int? TypeOfModule { get; set; }
        public int? TypeOfVisit { get; set; }

        [StringLength(500)]
        public string CampCode { get; set; }

        public int? DistrictId { get; set; }
        public int? BlockId { get; set; }
        public int? CLFId { get; set; }
        public int? PanchayatId { get; set; }

        [StringLength(500)]
        public string VOName { get; set; }

        public DateTime? CampDate { get; set; }
        public string Location { get; set; }

        [StringLength(500)]
        public string CRPName { get; set; }

        [StringLength(20)]
        public string CRPMobileNo { get; set; }

        public int? ParticipantMobilized { get; set; }
        public int? TotalScreened { get; set; }
        public int? TotalGlassesDistributed { get; set; }
        public int? PowerOfGlassId { get; set; }
        public string PhotoUploadPath { get; set; }
        public bool? IsActive { get; set; }

        [StringLength(130)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Keyless]
    [Table("TempMasterGlass")]
    public class TempMasterGlass
    {
        public string District { get; set; }
        public string Block { get; set; }
        public string Panchayat { get; set; }
        public string Federation { get; set; }
    }

    // -------------------------
    // DbContext
    // -------------------------
    public class GlassDbContext : IdentityDbContext<ApplicationUser>
    {
        public GlassDbContext(DbContextOptions<GlassDbContext> options) : base(options)
        {
        }

        public DbSet<MstBlock> MstBlock { get; set; }
        public DbSet<MstCLF> MstCLF { get; set; }
        public DbSet<MstDistrict> MstDistrict { get; set; }
        public DbSet<MstFederation> MstFederation { get; set; }
        public DbSet<MstGlass> MstGlass { get; set; }
        public DbSet<MstOccupation> MstOccupation { get; set; }
        public DbSet<MstPanchayat> MstPanchayat { get; set; }
        public DbSet<TblCamp> TblCamp { get; set; }
        public DbSet<TempMasterGlass> TempMasterGlass { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // TempMasterGlass has no key (it's a temp table) so mark keyless already by [Keyless]

            // Defaults and SQL-specific configurations can be added here if you need them.
        }
    }



}


