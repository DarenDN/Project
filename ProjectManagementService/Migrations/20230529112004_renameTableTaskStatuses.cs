using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementService.Migrations
{
    public partial class renameTableTaskStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatuses_StateId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskStatuses",
                table: "TaskStatuses");

            migrationBuilder.RenameTable(
                name: "TaskStatuses",
                newName: "TaskStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskStates",
                table: "TaskStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStates_StateId",
                table: "Tasks",
                column: "StateId",
                principalTable: "TaskStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStates_StateId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskStates",
                table: "TaskStates");

            migrationBuilder.RenameTable(
                name: "TaskStates",
                newName: "TaskStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskStatuses",
                table: "TaskStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatuses_StateId",
                table: "Tasks",
                column: "StateId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
