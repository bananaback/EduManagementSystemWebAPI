﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "user");
        }
    }
}
