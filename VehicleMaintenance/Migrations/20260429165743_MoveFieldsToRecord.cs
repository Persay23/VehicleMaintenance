using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class MoveFieldsToRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MaintenanceRecordComponents");

            migrationBuilder.DropColumn(
                name: "TechnicianName",
                table: "MaintenanceRecordComponents");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "MaintenanceRecordComponents");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "MaintenanceRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LaborDays",
                table: "MaintenanceRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "MaintenanceRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "MaintenanceRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicianName",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorOrShop",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "LaborDays",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "TechnicianName",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "VendorOrShop",
                table: "MaintenanceRecords");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MaintenanceRecordComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicianName",
                table: "MaintenanceRecordComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "MaintenanceRecordComponents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
