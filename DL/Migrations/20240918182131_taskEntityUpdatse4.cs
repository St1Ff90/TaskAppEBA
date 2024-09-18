using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class taskEntityUpdatse4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks",
                column: "UserId",
                principalTable: "MyUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks",
                column: "UserId",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
