using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementService.Migrations
{
    public partial class new_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsIdentities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateStart = table.Column<DateTime>(type: "date", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "date", nullable: false),
                    Finished = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "TaskStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AllowedUserRoles = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dashboards_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    SprintId = table.Column<Guid>(type: "uuid", nullable: true),
                    Importance = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStories_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    EstimationInTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EstimationInPoints = table.Column<int>(type: "integer", nullable: true),
                    SprintId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskStates_StateId",
                        column: x => x.StateId,
                        principalTable: "TaskStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "StateRelationships",
                columns: new[] { "Id", "StateCurrent", "StateNext" },
                values: new object[,]
                {
                    { new Guid("1222655c-a4fd-422b-8729-ef833f65083e"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be") },
                    { new Guid("15368c31-24e3-479f-ad63-3106b1e16c24"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f") },
                    { new Guid("28589618-0897-42c5-ac54-fd1b0eaa7c4c"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17") },
                    { new Guid("5461dc6b-dd8a-4841-afb6-96f1ee1f850a"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c") },
                    { new Guid("5931b098-9c18-4e30-9d72-d3420d04f6b8"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf") },
                    { new Guid("593abe35-120c-437b-a6af-72f5d7b7d847"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff") },
                    { new Guid("679bf0f5-ab5a-428a-ac9f-217a3f6c19f9"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814") },
                    { new Guid("6a6756b7-44a4-4753-bede-01a69e16c1d8"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be") },
                    { new Guid("6f740baa-8aae-4e11-9317-fbcb4bdbd3dc"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("82d25b59-881d-47b0-b991-fdac4e4069fc"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff") },
                    { new Guid("8627d14a-7e48-4c73-8ae9-ad858fdd260d"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("90047ab5-09d0-4830-84f3-55703d70cc31"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("987a396c-ecc0-4d40-b1f9-5579f6645791"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("a9008fbb-7653-46bb-a502-cb43902aaef2"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("abc3f2e9-ea6d-4e3d-9655-a7a9b17cff91"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("bb0cef36-747b-4fbb-baeb-80fb3f4f46a0"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("bcf0a183-4257-44ee-9ab9-699ac8b4bccf"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf") }
                });

            migrationBuilder.InsertData(
                table: "TaskStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f"), "Завершено" },
                    { new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), "Работа" },
                    { new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"), "Анализ" },
                    { new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"), "К тестированию" },
                    { new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), "Оценка" },
                    { new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), "К доработке" },
                    { new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), "К работе" },
                    { new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), "Тестирование" },
                    { new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), "Доработка" }
                });

            migrationBuilder.InsertData(
                table: "TaskTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1738a748-d9f2-4a92-8010-d69b9fb7fd67"), "Ошибка" },
                    { new Guid("30755127-b34c-4b6f-9e44-57fbd8306bbc"), "Пользовательская история" },
                    { new Guid("460509ef-ce6d-4b4d-a3cf-1c2161fe4e5e"), "Прочее" },
                    { new Guid("dc9b0e23-5c4a-40fb-83ec-691bcd405a83"), "Задача" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("b68e2f0d-c7e8-4ba2-b5ff-093ac717cb9d"), "Стандартный" },
                    { new Guid("ef7d3fc3-611f-46b0-98fd-5bf8e1005dec"), "Админ" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_ProjectId",
                table: "Dashboards",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StateId",
                table: "Tasks",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TypeId",
                table: "Tasks",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_SprintId",
                table: "UserStories",
                column: "SprintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropTable(
                name: "ProjectsIdentities");

            migrationBuilder.DropTable(
                name: "ProjectsRoles");

            migrationBuilder.DropTable(
                name: "StateRelationships");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserStories");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TaskStates");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "Sprints");
        }
    }
}
