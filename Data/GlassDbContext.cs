namespace GlassLP.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace GlassApp.Models
    {
        // -------------------------
        // Entity classes
        // -------------------------

        [Table("AspNetRoles")]
        public class AspNetRole
        {
            [Key]
            [StringLength(128)]
            public string Id { get; set; }

            [Required]
            [StringLength(256)]
            public string Name { get; set; }
        }

        [Table("AspNetUserClaims")]
        public class AspNetUserClaim
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(128)]
            public string UserId { get; set; }

            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }

            // Navigation
            public AspNetUser User { get; set; }
        }

        [Table("AspNetUserLogins")]
        public class AspNetUserLogin
        {
            [StringLength(128)]
            public string LoginProvider { get; set; }

            [StringLength(128)]
            public string ProviderKey { get; set; }

            [StringLength(128)]
            public string UserId { get; set; }

            // Navigation
            public AspNetUser User { get; set; }
        }

        [Table("AspNetUserRoles")]
        public class AspNetUserRole
        {
            [StringLength(128)]
            public string UserId { get; set; }

            [StringLength(128)]
            public string RoleId { get; set; }

            // Navigation
            public AspNetUser User { get; set; }
            public AspNetRole Role { get; set; }
        }

        [Table("AspNetUsers")]
        public class AspNetUser
        {
            [Key]
            [StringLength(128)]
            public string Id { get; set; }

            [StringLength(256)]
            public string Email { get; set; }

            public bool EmailConfirmed { get; set; }
            public string PasswordHash { get; set; }
            public string SecurityStamp { get; set; }
            public string PhoneNumber { get; set; }
            public bool PhoneNumberConfirmed { get; set; }
            public bool TwoFactorEnabled { get; set; }
            public DateTime? LockoutEndDateUtc { get; set; }
            public bool LockoutEnabled { get; set; }
            public int AccessFailedCount { get; set; }

            [Required]
            [StringLength(256)]
            public string UserName { get; set; }

            // Navigation collections
            public virtual ICollection<AspNetUserClaim> Claims { get; set; }
            public virtual ICollection<AspNetUserLogin> Logins { get; set; }
            public virtual ICollection<AspNetUserRole> Roles { get; set; }
        }

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
            public string CeatedBy { get; set; }
            public DateTime? CeatedOn { get; set; }
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
        public class GlassDbContext : DbContext
        {
            public GlassDbContext(DbContextOptions<GlassDbContext> options) : base(options)
            {
            }

            public DbSet<AspNetRole> AspNetRoles { get; set; }
            public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
            public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
            public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
            public DbSet<AspNetUser> AspNetUsers { get; set; }

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

                // composite keys
                modelBuilder.Entity<AspNetUserLogin>().HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
                modelBuilder.Entity<AspNetUserRole>().HasKey(r => new { r.UserId, r.RoleId });

                // relationships
                modelBuilder.Entity<AspNetUserClaim>()
                    .HasOne(c => c.User)
                    .WithMany(u => u.Claims)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<AspNetUserLogin>()
                    .HasOne(l => l.User)
                    .WithMany(u => u.Logins)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<AspNetUserRole>()
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.Roles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<AspNetUserRole>()
                    .HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // TempMasterGlass has no key (it's a temp table) so mark keyless already by [Keyless]

                // Defaults and SQL-specific configurations can be added here if you need them.
            }
        }



    }

}
