using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountDataSaver.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class _11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "UserAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_AuthorId",
                table: "UserAccounts",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Users_AuthorId",
                table: "UserAccounts",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Users_AuthorId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_AuthorId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "UserAccounts");
        }
    }
}
