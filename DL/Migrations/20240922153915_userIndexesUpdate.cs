using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class userIndexesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Добавляем индекс на поле Status
            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_Status",
                table: "UserTasks",
                column: "Status");

            // Добавляем индекс на поле DueDate
            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_DueDate",
                table: "UserTasks",
                column: "DueDate");

            // Добавляем индекс на поле Priority
            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_Priority",
                table: "UserTasks",
                column: "Priority");

            // Добавляем составной индекс на Priority и Status
            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_Priority_Status",
                table: "UserTasks",
                columns: new[] { "Priority", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаляем индекс на поле Status
            migrationBuilder.DropIndex(
                name: "IX_UserTasks_Status",
                table: "UserTasks");

            // Удаляем индекс на поле DueDate
            migrationBuilder.DropIndex(
                name: "IX_UserTasks_DueDate",
                table: "UserTasks");

            // Удаляем индекс на поле Priority
            migrationBuilder.DropIndex(
                name: "IX_UserTasks_Priority",
                table: "UserTasks");

            // Удаляем составной индекс на Priority и Status
            migrationBuilder.DropIndex(
                name: "IX_UserTasks_Priority_Status",
                table: "UserTasks");
        }
    }
}
