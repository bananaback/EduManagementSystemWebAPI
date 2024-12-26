using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_user_user_id",
                table: "refresh_token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_refresh_token",
                table: "refresh_token");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "refresh_token",
                newName: "refresh_tokens");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_token_user_id",
                table: "refresh_tokens",
                newName: "IX_refresh_tokens_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_refresh_tokens",
                table: "refresh_tokens",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_user_id",
                table: "refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_users_user_id",
                table: "refresh_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_refresh_tokens",
                table: "refresh_tokens");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "refresh_tokens",
                newName: "refresh_token");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_token",
                newName: "IX_refresh_token_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_refresh_token",
                table: "refresh_token",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_user_user_id",
                table: "refresh_token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
