using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleComponentId",
                table: "Predictions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions",
                column: "VehicleComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleComponentId",
                table: "Predictions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions",
                column: "VehicleComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId");
        }
    }
}
