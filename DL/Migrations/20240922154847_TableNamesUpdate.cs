using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class TableNamesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTasks_MyUsers_UserId",
                table: "MyTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyTasks",
                table: "MyTasks");

            migrationBuilder.RenameTable(
                name: "MyTasks",
                newName: "UserTasks");

            migrationBuilder.RenameIndex(
                name: "IX_MyTasks_UserId",
                table: "UserTasks",
                newName: "IX_UserTasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_DueDate",
                table: "UserTasks",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_Priority",
                table: "UserTasks",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_Status",
                table: "UserTasks",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_MyUsers_UserId",
                table: "UserTasks",
                column: "UserId",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_MyUsers_UserId",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_DueDate",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_Priority",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_Status",
                table: "UserTasks");

            migrationBuilder.RenameTable(
                name: "UserTasks",
                newName: "MyTasks");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_UserId",
                table: "MyTasks",
                newName: "IX_MyTasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyTasks",
                table: "MyTasks",
                column: "Id");

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
