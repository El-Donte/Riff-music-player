using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiffBackend.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailVerificationTokens",
                table: "EmailVerificationTokens");

            migrationBuilder.RenameTable(
                name: "EmailVerificationTokens",
                newName: "emailVerificationTokens");

            migrationBuilder.AddColumn<bool>(
                name: "email_verified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_emailVerificationTokens",
                table: "emailVerificationTokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_emailVerificationTokens_UserId",
                table: "emailVerificationTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_emailVerificationTokens_users_UserId",
                table: "emailVerificationTokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_emailVerificationTokens_users_UserId",
                table: "emailVerificationTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_emailVerificationTokens",
                table: "emailVerificationTokens");

            migrationBuilder.DropIndex(
                name: "IX_emailVerificationTokens_UserId",
                table: "emailVerificationTokens");

            migrationBuilder.DropColumn(
                name: "email_verified",
                table: "users");

            migrationBuilder.RenameTable(
                name: "emailVerificationTokens",
                newName: "EmailVerificationTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailVerificationTokens",
                table: "EmailVerificationTokens",
                column: "Id");
        }
    }
}
