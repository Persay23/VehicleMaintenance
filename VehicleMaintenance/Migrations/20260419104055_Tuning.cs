using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class Tuning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecords_Predictions_PredictionId",
                table: "MaintenanceRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentComponentId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRecords_PredictionId",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "PredictionId",
                table: "MaintenanceRecords");

            migrationBuilder.RenameColumn(
                name: "ComponentId",
                table: "VehicleComponents",
                newName: "VehicleComponentId");

            migrationBuilder.RenameColumn(
                name: "VehicleComponentComponentId",
                table: "Predictions",
                newName: "VehicleComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_VehicleComponentComponentId",
                table: "Predictions",
                newName: "IX_Predictions_VehicleComponentId");

            migrationBuilder.RenameColumn(
                name: "VendorOrShop",
                table: "MaintenanceRecordComponents",
                newName: "Vendor");

            migrationBuilder.RenameColumn(
                name: "ChangeType",
                table: "MaintenanceRecordComponents",
                newName: "ComponentChangeType");

            migrationBuilder.AddColumn<int>(
                name: "PredictionId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleComponentBrand",
                table: "VehicleComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleComponentName",
                table: "VehicleComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Predictions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Predictions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OldState",
                table: "MaintenanceRecordComponents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewState",
                table: "MaintenanceRecordComponents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "FuelEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FuelEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PredictionId",
                table: "Vehicles",
                column: "PredictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions",
                column: "VehicleComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "VehicleComponentId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Predictions_PredictionId",
                table: "Vehicles",
                column: "PredictionId",
                principalTable: "Predictions",
                principalColumn: "PredictionId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Predictions_PredictionId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_PredictionId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PredictionId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleComponentBrand",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "VehicleComponentName",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "FuelEntries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FuelEntries");

            migrationBuilder.RenameColumn(
                name: "VehicleComponentId",
                table: "VehicleComponents",
                newName: "ComponentId");

            migrationBuilder.RenameColumn(
                name: "VehicleComponentId",
                table: "Predictions",
                newName: "VehicleComponentComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_VehicleComponentId",
                table: "Predictions",
                newName: "IX_Predictions_VehicleComponentComponentId");

            migrationBuilder.RenameColumn(
                name: "Vendor",
                table: "MaintenanceRecordComponents",
                newName: "VendorOrShop");

            migrationBuilder.RenameColumn(
                name: "ComponentChangeType",
                table: "MaintenanceRecordComponents",
                newName: "ChangeType");

            migrationBuilder.AddColumn<int>(
                name: "PredictionId",
                table: "MaintenanceRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OldState",
                table: "MaintenanceRecordComponents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NewState",
                table: "MaintenanceRecordComponents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_PredictionId",
                table: "MaintenanceRecords",
                column: "PredictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecords_Predictions_PredictionId",
                table: "MaintenanceRecords",
                column: "PredictionId",
                principalTable: "Predictions",
                principalColumn: "PredictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_VehicleComponents_VehicleComponentComponentId",
                table: "Predictions",
                column: "VehicleComponentComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "ComponentId");
        }
    }
}
