using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlassLP.Migrations
{
    /// <inheritdoc />
    public partial class InitialGlass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mst_Block",
                columns: table => new
                {
                    BlockId_pk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId_fk = table.Column<int>(type: "int", nullable: true),
                    BlockName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Block", x => x.BlockId_pk);
                });

            migrationBuilder.CreateTable(
                name: "mst_CLF",
                columns: table => new
                {
                    pk_CLFId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLFName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_CLF", x => x.pk_CLFId);
                });

            migrationBuilder.CreateTable(
                name: "mst_District",
                columns: table => new
                {
                    DistrictId_pk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_District", x => x.DistrictId_pk);
                });

            migrationBuilder.CreateTable(
                name: "mst_Federation",
                columns: table => new
                {
                    FederationId_pk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId_fk = table.Column<int>(type: "int", nullable: true),
                    BlockId_fk = table.Column<int>(type: "int", nullable: true),
                    PanchayatId_fk = table.Column<int>(type: "int", nullable: true),
                    FederationName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Federation", x => x.FederationId_pk);
                });

            migrationBuilder.CreateTable(
                name: "mst_Glass",
                columns: table => new
                {
                    pk_Glassid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PowerOfGlass = table.Column<int>(type: "int", nullable: true),
                    GlassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Glass", x => x.pk_Glassid);
                });

            migrationBuilder.CreateTable(
                name: "mst_Occupation",
                columns: table => new
                {
                    pk_OccupationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OccupatioName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Occupation", x => x.pk_OccupationId);
                });

            migrationBuilder.CreateTable(
                name: "mst_Panchayat",
                columns: table => new
                {
                    PanchayatId_pk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId_fk = table.Column<int>(type: "int", nullable: true),
                    BlockId_fk = table.Column<int>(type: "int", nullable: true),
                    PanchayatName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_Panchayat", x => x.PanchayatId_pk);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Camp",
                columns: table => new
                {
                    CampId_pk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfModule = table.Column<int>(type: "int", nullable: true),
                    TypeOfVisit = table.Column<int>(type: "int", nullable: true),
                    CampCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    BlockId = table.Column<int>(type: "int", nullable: true),
                    CLFId = table.Column<int>(type: "int", nullable: true),
                    PanchayatId = table.Column<int>(type: "int", nullable: true),
                    VOName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CampDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CRPName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CRPMobileNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParticipantMobilized = table.Column<int>(type: "int", nullable: true),
                    TotalScreened = table.Column<int>(type: "int", nullable: true),
                    TotalGlassesDistributed = table.Column<int>(type: "int", nullable: true),
                    PowerOfGlassId = table.Column<int>(type: "int", nullable: true),
                    PhotoUploadPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Camp", x => x.CampId_pk);
                });

            migrationBuilder.CreateTable(
                name: "TempMasterGlass",
                columns: table => new
                {
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Panchayat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Federation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mst_Block");

            migrationBuilder.DropTable(
                name: "mst_CLF");

            migrationBuilder.DropTable(
                name: "mst_District");

            migrationBuilder.DropTable(
                name: "mst_Federation");

            migrationBuilder.DropTable(
                name: "mst_Glass");

            migrationBuilder.DropTable(
                name: "mst_Occupation");

            migrationBuilder.DropTable(
                name: "mst_Panchayat");

            migrationBuilder.DropTable(
                name: "tbl_Camp");

            migrationBuilder.DropTable(
                name: "TempMasterGlass");
        }
    }
}
