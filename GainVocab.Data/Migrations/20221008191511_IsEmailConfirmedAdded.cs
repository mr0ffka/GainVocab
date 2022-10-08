using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.Data.Migrations
{
    public partial class IsEmailConfirmedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2a21ea9-b61f-4888-8d73-09320086d14c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc7717df-16ea-4e05-9be7-79259978d718");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "06010d62-1b7a-49af-a3f6-1c0790b8fc96", "f3224436-1ae4-442f-8dd3-067c281e2264", "Administrator", "ADMINISTRATOR" },
                    { "74b9a530-9483-45e8-81f6-10c562066e46", "37ea1bba-a1c3-4002-906a-1b08792a6132", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06010d62-1b7a-49af-a3f6-1c0790b8fc96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74b9a530-9483-45e8-81f6-10c562066e46");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f2a21ea9-b61f-4888-8d73-09320086d14c", "790a33cf-f59d-4a59-baf1-6e81de2fa725", "Administrator", "ADMINISTRATOR" },
                    { "fc7717df-16ea-4e05-9be7-79259978d718", "e5ac6333-a656-4e9e-b69b-08f33ce66622", "User", "USER" }
                });
        }
    }
}
