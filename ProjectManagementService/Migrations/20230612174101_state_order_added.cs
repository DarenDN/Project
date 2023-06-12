using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementService.Migrations
{
    public partial class state_order_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("1222655c-a4fd-422b-8729-ef833f65083e"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("15368c31-24e3-479f-ad63-3106b1e16c24"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("28589618-0897-42c5-ac54-fd1b0eaa7c4c"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("5461dc6b-dd8a-4841-afb6-96f1ee1f850a"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("5931b098-9c18-4e30-9d72-d3420d04f6b8"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("593abe35-120c-437b-a6af-72f5d7b7d847"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("679bf0f5-ab5a-428a-ac9f-217a3f6c19f9"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("6a6756b7-44a4-4753-bede-01a69e16c1d8"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("6f740baa-8aae-4e11-9317-fbcb4bdbd3dc"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("82d25b59-881d-47b0-b991-fdac4e4069fc"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("8627d14a-7e48-4c73-8ae9-ad858fdd260d"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("90047ab5-09d0-4830-84f3-55703d70cc31"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("987a396c-ecc0-4d40-b1f9-5579f6645791"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("a9008fbb-7653-46bb-a502-cb43902aaef2"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("abc3f2e9-ea6d-4e3d-9655-a7a9b17cff91"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("bb0cef36-747b-4fbb-baeb-80fb3f4f46a0"));

            migrationBuilder.DeleteData(
                table: "StateRelationships",
                keyColumn: "Id",
                keyValue: new Guid("bcf0a183-4257-44ee-9ab9-699ac8b4bccf"));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "TaskStates",
                type: "integer",
                nullable: true);

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

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f"),
                column: "Order",
                value: 8);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"),
                column: "Order",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"),
                column: "Order",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"),
                column: "Order",
                value: 4);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"),
                column: "Order",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"),
                column: "Order",
                value: 6);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"),
                column: "Order",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"),
                column: "Order",
                value: 5);

            migrationBuilder.UpdateData(
                table: "TaskStates",
                keyColumn: "Id",
                keyValue: new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"),
                column: "Order",
                value: 7);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Order",
                table: "TaskStates");

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
        }
    }
}
