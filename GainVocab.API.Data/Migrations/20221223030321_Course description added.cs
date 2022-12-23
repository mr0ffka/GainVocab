using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class Coursedescriptionadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IssueEntityId",
                table: "SupportIssue",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.AlterColumn<long>(
                name: "IssueEntityId",
                table: "SupportIssue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
