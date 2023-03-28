using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementService.Migrations
{
    public partial class InitalCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatusModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatusModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypeModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypeModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DashboardModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AllowedUserTypes = table.Column<int[]>(type: "integer[]", nullable: false),
                    ProjectID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DashboardModels_ProjectModels_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "ProjectModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RegisterTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserModels_UserRoleModels_RoleID",
                        column: x => x.RoleID,
                        principalTable: "UserRoleModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskModels",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    DashboardID = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeID = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskModels_DashboardModels_DashboardID",
                        column: x => x.DashboardID,
                        principalTable: "DashboardModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskModels_TaskStatusModels_StatusID",
                        column: x => x.StatusID,
                        principalTable: "TaskStatusModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskModels_TaskTypeModels_TypeID",
                        column: x => x.TypeID,
                        principalTable: "TaskTypeModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardModels_ProjectID",
                table: "DashboardModels",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_DashboardID",
                table: "TaskModels",
                column: "DashboardID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_StatusID",
                table: "TaskModels",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskModels_TypeID",
                table: "TaskModels",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_UserModels_RoleID",
                table: "UserModels",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskModels");

            migrationBuilder.DropTable(
                name: "UserModels");

            migrationBuilder.DropTable(
                name: "DashboardModels");

            migrationBuilder.DropTable(
                name: "TaskStatusModels");

            migrationBuilder.DropTable(
                name: "TaskTypeModels");

            migrationBuilder.DropTable(
                name: "UserRoleModels");

            migrationBuilder.DropTable(
                name: "ProjectModels");
        }
    }
}
