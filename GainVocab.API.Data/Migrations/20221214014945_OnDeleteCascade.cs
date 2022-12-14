using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class OnDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_LanguageFromId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_LanguageToId",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_LanguageFromId",
                table: "Courses",
                column: "LanguageFromId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_LanguageToId",
                table: "Courses",
                column: "LanguageToId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_LanguageFromId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_LanguageToId",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_LanguageFromId",
                table: "Courses",
                column: "LanguageFromId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_LanguageToId",
                table: "Courses",
                column: "LanguageToId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
