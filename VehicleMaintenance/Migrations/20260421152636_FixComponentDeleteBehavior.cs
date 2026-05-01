using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class FixComponentDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecordComponents_VehicleComponents_ComponentId",
                table: "MaintenanceRecordComponents");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecordComponents_VehicleComponents_ComponentId",
                table: "MaintenanceRecordComponents",
                column: "ComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecordComponents_VehicleComponents_ComponentId",
                table: "MaintenanceRecordComponents");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecordComponents_VehicleComponents_ComponentId",
                table: "MaintenanceRecordComponents",
                column: "ComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
