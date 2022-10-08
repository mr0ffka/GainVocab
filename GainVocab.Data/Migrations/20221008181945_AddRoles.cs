using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.Data.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f2a21ea9-b61f-4888-8d73-09320086d14c", "790a33cf-f59d-4a59-baf1-6e81de2fa725", "Administrator", "ADMINISTRATOR" },
                    { "fc7717df-16ea-4e05-9be7-79259978d718", "e5ac6333-a656-4e9e-b69b-08f33ce66622", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2a21ea9-b61f-4888-8d73-09320086d14c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc7717df-16ea-4e05-9be7-79259978d718");
        }
    }
}
