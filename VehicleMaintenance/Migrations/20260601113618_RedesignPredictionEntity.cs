using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class RedesignPredictionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "AiConfidenceScore",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiReasoning",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiRecommendation",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "ComponentType",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "PredictedServiceDate",
                table: "Predictions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Predictions",
                newName: "Urgency");

            migrationBuilder.RenameColumn(
                name: "AiRemainingKm",
                table: "Predictions",
                newName: "EstimatedRemainingKm");

            migrationBuilder.RenameColumn(
                name: "AiGeneratedAt",
                table: "Predictions",
                newName: "SuggestedByDate");

            migrationBuilder.RenameColumn(
                name: "AiEstimatedDate",
                table: "Predictions",
                newName: "IgnoredAt");

            migrationBuilder.AddColumn<double>(
                name: "AiConfidenceScore",
                table: "VehicleComponents",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AiEstimatedNextServiceDate",
                table: "VehicleComponents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AiEstimatedRemainingKm",
                table: "VehicleComponents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AiGeneratedAt",
                table: "VehicleComponents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AiHealthPercent",
                table: "VehicleComponents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiReasoning",
                table: "VehicleComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiRecommendation",
                table: "VehicleComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleComponentId",
                table: "Predictions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ConfidenceScore",
                table: "Predictions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiConfidenceScore",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "AiEstimatedNextServiceDate",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "AiEstimatedRemainingKm",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "AiGeneratedAt",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "AiHealthPercent",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "AiReasoning",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "AiRecommendation",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Predictions");

            migrationBuilder.RenameColumn(
                name: "Urgency",
                table: "Predictions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SuggestedByDate",
                table: "Predictions",
                newName: "AiGeneratedAt");

            migrationBuilder.RenameColumn(
                name: "IgnoredAt",
                table: "Predictions",
                newName: "AiEstimatedDate");

            migrationBuilder.RenameColumn(
                name: "EstimatedRemainingKm",
                table: "Predictions",
                newName: "AiRemainingKm");

            migrationBuilder.AddColumn<int>(
                name: "PredictionId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleComponentId",
                table: "Predictions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ConfidenceScore",
                table: "Predictions",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AiConfidenceScore",
                table: "Predictions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiReasoning",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AiRecommendation",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComponentType",
                table: "Predictions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PredictedServiceDate",
                table: "Predictions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PredictionId",
                table: "Vehicles",
                column: "PredictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Predictions_PredictionId",
                table: "Vehicles",
                column: "PredictionId",
                principalTable: "Predictions",
                principalColumn: "PredictionId");
        }
    }
}
