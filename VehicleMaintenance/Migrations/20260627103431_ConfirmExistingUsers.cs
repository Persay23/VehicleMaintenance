using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmExistingUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Grandfather in everyone who registered before email confirmation existed, so enabling
            // RequireConfirmedEmail doesn't lock out existing accounts (their emails may be stale/fake).
            // New signups (created after this migration) still require confirmation.
            migrationBuilder.Sql("UPDATE [AspNetUsers] SET [EmailConfirmed] = 1 WHERE [EmailConfirmed] = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
