using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteOnVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                table: "MaintenanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Vehicles_VehicleId",
                table: "Predictions");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Vehicles_VehicleId",
                table: "Predictions",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                table: "MaintenanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_Vehicles_VehicleId",
                table: "Predictions");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_Vehicles_VehicleId",
                table: "Predictions",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId");
        }
    }
}
