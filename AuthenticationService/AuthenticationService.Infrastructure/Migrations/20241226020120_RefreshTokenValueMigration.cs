using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenValueMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token",
                table: "refresh_token",
                newName: "token_value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token_value",
                table: "refresh_token",
                newName: "token");
        }
    }
}
