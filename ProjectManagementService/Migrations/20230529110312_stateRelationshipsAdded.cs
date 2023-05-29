using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementService.Migrations
{
    public partial class stateRelationshipsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Tasks",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                newName: "IX_Tasks_StateId");

            migrationBuilder.CreateTable(
                name: "StateRelationships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StateCurrent = table.Column<Guid>(type: "uuid", nullable: false),
                    StateNext = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateRelationships", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatuses_StateId",
                table: "Tasks",
                column: "StateId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatuses_StateId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "StateRelationships");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Tasks",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_StateId",
                table: "Tasks",
                newName: "IX_Tasks_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
