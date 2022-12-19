using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class CourseDataExamplecascadedelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDataExample_CourseDataId",
                table: "CourseDataExample");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDataExample_CourseDataId",
                table: "CourseDataExample",
                column: "CourseDataId",
                principalTable: "CourseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDataExample_CourseDataId",
                table: "CourseDataExample");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDataExample_CourseDataId",
                table: "CourseDataExample",
                column: "CourseDataId",
                principalTable: "CourseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
