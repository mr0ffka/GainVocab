using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class AddCourseFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Languages_LanguageFromId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Languages_LanguageToId",
                table: "Courses");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_LanguageFromId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_LanguageToId",
                table: "Courses");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Languages_LanguageFromId",
                table: "Courses",
                column: "LanguageFromId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Languages_LanguageToId",
                table: "Courses",
                column: "LanguageToId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
