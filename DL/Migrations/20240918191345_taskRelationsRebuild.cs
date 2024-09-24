using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class taskRelationsRebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "MyTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "MyTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MyTasks",
                type: "int",
                nullable: true);

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
                name: "IX_MyTasks_UserId",
                table: "MyTasks");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "MyTasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "MyTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MyTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MyTasks");
        }
    }
}
