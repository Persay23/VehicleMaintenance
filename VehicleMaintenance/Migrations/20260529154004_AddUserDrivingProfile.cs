using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDrivingProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDrivingProfiles",
                columns: table => new
                {
                    UserDrivingProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnnualKm = table.Column<int>(type: "int", nullable: false),
                    PrimaryUsage = table.Column<int>(type: "int", nullable: false),
                    DrivingStyle = table.Column<int>(type: "int", nullable: false),
                    UsagePattern = table.Column<int>(type: "int", nullable: false),
                    ClimateZone = table.Column<int>(type: "int", nullable: false),
                    ParkingType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDrivingProfiles", x => x.UserDrivingProfileId);
                    table.ForeignKey(
                        name: "FK_UserDrivingProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDrivingProfiles_UserId",
                table: "UserDrivingProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDrivingProfiles");
        }
    }
}
