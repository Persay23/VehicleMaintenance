using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddAiFieldsToPrediction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AiConfidenceScore",
                table: "Predictions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AiEstimatedDate",
                table: "Predictions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AiGeneratedAt",
                table: "Predictions",
                type: "datetime2",
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
                name: "AiRemainingKm",
                table: "Predictions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiConfidenceScore",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiEstimatedDate",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiGeneratedAt",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiReasoning",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiRecommendation",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "AiRemainingKm",
                table: "Predictions");
        }
    }
}
