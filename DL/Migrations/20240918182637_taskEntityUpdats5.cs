using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class taskEntityUpdats5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks");

            migrationBuilder.DropIndex(
                name: "IX_MyTasks_UserId",
                table: "MyTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MyTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "MyTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyTasks_UserId",
                table: "MyTasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks",
                column: "UserId",
                principalTable: "MyUsers",
                principalColumn: "Id");
        }
    }
}
