using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class addrelationtosupportissuewithcourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IssueEntityId",
                table: "SupportIssue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportIssue_IssueEntityId",
                table: "SupportIssue",
                column: "IssueEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportIssue_IssueEntityId",
                table: "SupportIssue",
                column: "IssueEntityId",
                principalTable: "CourseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportIssue_IssueEntityId",
                table: "SupportIssue");

            migrationBuilder.DropIndex(
                name: "IX_SupportIssue_IssueEntityId",
                table: "SupportIssue");

            migrationBuilder.AlterColumn<string>(
                name: "IssueEntityId",
                table: "SupportIssue",
                type: "text",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
