using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class userUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "MyUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MyUsers_Username",
                table: "MyUsers",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks",
                column: "UserId",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks");

            migrationBuilder.DropIndex(
                name: "IX_MyUsers_Username",
                table: "MyUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "MyUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks",
                column: "UserId",
                principalTable: "MyUsers",
                principalColumn: "Id");
        }
    }
}
