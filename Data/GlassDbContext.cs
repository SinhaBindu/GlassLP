
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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
        public int? ModelType { get; set; }
        [StringLength(500)]
        public string? BlockName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(130)]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_CLF")]
    public class MstCLF
    {
        [Key]
        public int pk_CLFId { get; set; }

        [StringLength(100)]
        public string? CLFName { get; set; }
        public bool? IsActive { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_District")]
    public class MstDistrict
    {
        [Key]
        public int DistrictId_pk { get; set; }
        public int? ModelType { get; set; }
        [StringLength(500)]
        public string DistrictName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(130)]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string? UpdatedBy { get; set; }
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
        public string? FederationName { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(130)]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [Table("mst_Glass")]
    public class MstGlass
    {
        [Key]
        public int pk_Glassid { get; set; }
        public int TypeOfModuleId { get; set; }  
        public int PowerOfGlassId { get; set; }
        public int NoofGlasses { get; set; }
        [Range(5,2)]
        public decimal? PerGlassCost { get; set; } = 0;
        [Range(5, 2)]
        public decimal? TotalGlassCost { get; set; } = 0;
        [Range(5, 2)]
        public decimal? AdvertisingCost { get; set; } = 0;//m2
        [Range(5, 2)]
        public decimal? Storagemiscellaneouscost { get; set; } = 0;//m2
        public bool? IsActive { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Version { get; set; }
        public DateTime SynDate { get; set; }
        public string uuid { get; set; }
        public int? Availableglassinstock { get; set; }
        public int? TotalDistributedGlass { get; set; }
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
		[ValidateNever]
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
        public int? VEId { get; set; }

        public int? ParticipantMobilized { get; set; }
        public int? TotalScreened { get; set; }
        public int? TotalGlassesDistributed { get; set; }
        public int? PowerOfGlassId { get; set; }
        public string PhotoUploadPath { get; set; }
        public bool? IsActive { get; set; }

        [StringLength(130)]
        [ValidateNever]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        [ValidateNever]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Version { get; set; }
        public DateTime SynDate { get; set; }
        public string uuid { get; set; }
    }

    [Table("mst_Vendors")]//Vision Entrepreneur 
    public class MstVendor
    {
        [Key]
        public int pk_VendorsId { get; set; } = 0;

        [DisplayName("Business Mentor Name")]
        public string? BusinessMentorName { get; set; }

        [DisplayName("VE Name")]
        public string? VEName { get; set; }

        [DisplayName("CLF Name")]
        public int CLFId { get; set; }

        [DisplayName("AOP Name")]
        public string? AOPName { get; set; }

        [DisplayName("Contact Number")]
        public string? ContactNumber { get; set; }

        [DisplayName("Total Sales Amount")]
        public string? TotalSalesAmount { get; set; }
        public bool? IsActive { get; set; }

        [StringLength(130)]
        [ValidateNever]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        [ValidateNever]
        public string? UpdatedBy { get; set; }
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

    [Table("tbl_PaticipantM1")]
    public class TblPaticipantM1
    {
        [Key]
        public int ParticipantId_pk { get; set; } = 0;
        public int CampId_fk { get; set; }
        public string? ParticipantName { get; set; }
        public string? MobileNo { get; set; }
        public int Age { get; set; }
        public string? SHGName { get; set; }
        public int OccupationId { get; set; }

		public string? Occupation_Others { get; set; }
        public int VisionIssueIdentifiedId { get; set; }
        public int TypeofVisionIssueId { get; set; }
        public int GlassesProvidedId { get; set; }
        public int PowerofGlassId { get; set; }
        public int FeedbackonComfort { get; set; }
        public string? Remarks { get; set; }
        public int FollowupRequiredId { get; set; }
        public int DigitalConsentId { get; set; }
        public bool? IsActive { get; set; }

        [StringLength(130)]
        [ValidateNever]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(130)]
        [ValidateNever]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Version { get; set; }
        public DateTime SynDate { get; set; }
        public string uuid { get; set; }
    }

	[Table("tbl_PaticipantM2")]
	public class TblPaticipantM2
	{
		[Key]
		public int ParticipantId_pk { get; set; } = 0;
		[DisplayName("Type of Participant")]
		public int TypeofParticipantId { get; set; }
		
		[DisplayName("Camp Code")]
		public int CampId_fk { get; set; }

		[DisplayName("District")]
		public int? DistrictId { get; set; }

		[DisplayName("Block")]
		public int? BlockId { get; set; }

		[DisplayName("CLF")]
		public int? CLFId { get; set; }

		[DisplayName("Panchayat")]
		public int? PanchayatId { get; set; }

		[DisplayName("Participant Name")]
		public string? ParticipantName { get; set; }

		[DisplayName("Mobile No")]
		public string? MobileNo { get; set; }

		[DisplayName("Age")]
		public int Age { get; set; }

		[DisplayName("Screening Date")]
		public DateTime? ScreeningDate { get; set; }

		[DisplayName("SHG Name")]
		public string? SHGName { get; set; }

		[DisplayName("Occupation")]
		public int OccupationId { get; set; }

		[DisplayName("Occupation Others")]
		[ValidateNever]
		public string? Occupation_Others { get; set; }

		[DisplayName("Vision Issue Identified")]
		public int VisionIssueIdentifiedId { get; set; }

		[DisplayName("Type of Vision Issue")]
		public int TypeofVisionIssueId { get; set; }
        [DisplayName("Type of Vision Issue (Other)")]
        public string TypeofVisionIssue_Others { get; set; }

		[DisplayName("Glasses Sold")]
		public int GlassesSold { get; set; }

		[DisplayName("Power of Glass")]
		public int PowerofGlassId { get; set; }

		[DisplayName("Feedbackon Comfort")]
		public int FeedbackonComfort { get; set; }

		[DisplayName("Remarks")]
		public string? Remarks { get; set; }

		[DisplayName("Referral Required")]
		public int FollowupRequiredId { get; set; }

		[DisplayName("Digital Consent")]
		public int DigitalConsentId { get; set; }

		[DisplayName("Location")]
		public int Location { get; set; }

		[DisplayName("Screening Cost")]
		public int ScreeningCost { get; set; }

		[DisplayName("Glasses Cost")]
		public int GlassesCost { get; set; }

		[DisplayName("Remarks/Action Taken")]
		public string? RemarksActionTaken { get; set; }
		public bool? IsActive { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
        public string Version { get; set; }
        public DateTime SynDate { get; set; }
        public string uuid { get; set; }
    }

	[Table("mst_PowerGlass")]
	public class MstPowerGlasses
	{
		[Key]
		public int pk_PowerGlassId { get; set; }

		[StringLength(100)]
		public string? PowerofGlass { get; set; }

		public bool? IsActive { get; set; }
		[StringLength(100)]
		public string? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		[StringLength(100)]
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
        public string Version { get; set; }
        public DateTime SynDate { get; set; }
        public string uuid { get; set; }
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
        public DbSet<MstVendor> MstVendor { get; set; }
        public DbSet<TblPaticipantM1> TblPaticipantM1 { get; set; }
		public DbSet<TblPaticipantM2> TblPaticipantM2 { get; set; }
		public DbSet<MstPowerGlasses> MstPowerGlasses { get; set; }
		public DbSet<TempMasterGlass> TempMasterGlass { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // TempMasterGlass has no key (it's a temp table) so mark keyless already by [Keyless]

            // Defaults and SQL-specific configurations can be added here if you need them.
        }
    }



}


