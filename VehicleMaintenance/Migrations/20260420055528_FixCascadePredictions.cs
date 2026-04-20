using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadePredictions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions",
                column: "VehicleComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions",
                column: "VehicleComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
