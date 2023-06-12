using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementService.Migrations
{
    public partial class sprint_date_created_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("2b8d5fc5-170f-409e-a49c-1f472ab4095f"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("50133267-24d8-473a-9502-91a15613d852"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("57701f91-fea4-40f4-8c25-68939f2703d6"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("5fb03a10-19cd-40be-a8f3-0ae4ce3d8644"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("687782db-73f2-400e-a179-6dd6b1a90f52"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("717ac234-3cc0-484d-8f05-b4a41459a7ab"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("999d3e23-224f-484d-984e-b1d747ebf50e"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("9a0e8ccd-f368-4659-bab9-56824d8727fd"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("a0f99de4-16f5-44e3-99e9-ff4e2b20e80c"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("ab1d1a8a-0009-46ba-b539-b9ec6f7e9439"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("b92c18e9-872e-4f01-aa07-3b9f0951ea1e"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("c8c62654-d646-439c-a443-8c9f5f964bce"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("c9557731-7df0-4b14-a8b2-5593846d0621"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("c9724f8f-973e-4a76-8e21-88d0b77470a4"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("e946eb51-a59a-47b8-9b59-a23066911db3"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("edf99163-e97f-4135-9964-177a31fe5c84"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("f4790bbb-9bcb-4783-b5ed-448090e64e9c"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Sprints",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "StateRelationships",
                columns: new[] { "Id", "StateCurrent", "StateNext" },
                values: new object[,]
                {
                    { new Guid("046005b8-90fa-4d7a-abdc-9db422306749"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("04d8d714-970a-4623-b77a-fa010e4f1050"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f") },
                    { new Guid("149679ea-0a4d-4e10-b0b7-feec039e31d2"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("159ba830-e998-459d-af2b-97a981878e5e"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17") },
                    { new Guid("3ded1351-3f04-4718-ab9e-0d63f72ab6e5"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("461b8dcc-163d-40c3-80a0-a4f956cb2aa7"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("49165ac7-bf14-430a-819f-70e778e8b19d"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff") },
                    { new Guid("4ae7d5cc-8a43-46be-9e3f-cc9de6cdba4e"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf") },
                    { new Guid("5ab4005c-e2c3-43f4-bfa2-09845c04b863"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be") },
                    { new Guid("6475ed4c-cb2d-4bd5-ab3c-e5f9f4f8391d"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c") },
                    { new Guid("74e27698-dbb5-4d72-8174-4fc1058b1dab"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff") },
                    { new Guid("a4dca5f1-ac48-4804-afd4-bc6cf0cae532"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("a64379ad-7398-4979-8ec1-94c3085f2fd8"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("aab7f93d-feac-43d7-8742-e698111af2ec"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be") },
                    { new Guid("c52e3ca0-3d16-42e1-98ba-9e4bde539bc5"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf") },
                    { new Guid("c66438b7-bc9e-4ce8-8a4a-24b4ce2d6e03"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("d4c6c12b-9cb6-441c-b532-d97ff8ad5071"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("046005b8-90fa-4d7a-abdc-9db422306749"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("04d8d714-970a-4623-b77a-fa010e4f1050"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("149679ea-0a4d-4e10-b0b7-feec039e31d2"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("159ba830-e998-459d-af2b-97a981878e5e"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("3ded1351-3f04-4718-ab9e-0d63f72ab6e5"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("461b8dcc-163d-40c3-80a0-a4f956cb2aa7"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("49165ac7-bf14-430a-819f-70e778e8b19d"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("4ae7d5cc-8a43-46be-9e3f-cc9de6cdba4e"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("5ab4005c-e2c3-43f4-bfa2-09845c04b863"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("6475ed4c-cb2d-4bd5-ab3c-e5f9f4f8391d"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("74e27698-dbb5-4d72-8174-4fc1058b1dab"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("a4dca5f1-ac48-4804-afd4-bc6cf0cae532"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("a64379ad-7398-4979-8ec1-94c3085f2fd8"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("aab7f93d-feac-43d7-8742-e698111af2ec"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("c52e3ca0-3d16-42e1-98ba-9e4bde539bc5"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("c66438b7-bc9e-4ce8-8a4a-24b4ce2d6e03"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("d4c6c12b-9cb6-441c-b532-d97ff8ad5071"));

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Sprints");

            migrationBuilder.InsertData(
                table: "StateRelationships",
                columns: new[] { "Id", "StateCurrent", "StateNext" },
                values: new object[,]
                {
                    { new Guid("2b8d5fc5-170f-409e-a49c-1f472ab4095f"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("50133267-24d8-473a-9502-91a15613d852"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be") },
                    { new Guid("57701f91-fea4-40f4-8c25-68939f2703d6"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17") },
                    { new Guid("5fb03a10-19cd-40be-a8f3-0ae4ce3d8644"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("687782db-73f2-400e-a179-6dd6b1a90f52"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff") },
                    { new Guid("717ac234-3cc0-484d-8f05-b4a41459a7ab"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("999d3e23-224f-484d-984e-b1d747ebf50e"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604") },
                    { new Guid("9a0e8ccd-f368-4659-bab9-56824d8727fd"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("a0f99de4-16f5-44e3-99e9-ff4e2b20e80c"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814") },
                    { new Guid("ab1d1a8a-0009-46ba-b539-b9ec6f7e9439"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be") },
                    { new Guid("b92c18e9-872e-4f01-aa07-3b9f0951ea1e"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f") },
                    { new Guid("c8c62654-d646-439c-a443-8c9f5f964bce"), new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf") },
                    { new Guid("c9557731-7df0-4b14-a8b2-5593846d0621"), new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") },
                    { new Guid("c9724f8f-973e-4a76-8e21-88d0b77470a4"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff") },
                    { new Guid("e946eb51-a59a-47b8-9b59-a23066911db3"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf") },
                    { new Guid("edf99163-e97f-4135-9964-177a31fe5c84"), new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"), new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c") },
                    { new Guid("f4790bbb-9bcb-4783-b5ed-448090e64e9c"), new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"), new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8") }
                });
        }
    }
}
