using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class RenameCurrentMileageToInstalledAtVehicleMileage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentMileage",
                table: "VehicleComponents",
                newName: "InstalledAtVehicleMileage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstalledAtVehicleMileage",
                table: "VehicleComponents",
                newName: "CurrentMileage");
        }
    }
}
