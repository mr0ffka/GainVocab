using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class AddCourseProgressv9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APIUserCourse_CourseProgress_CourseProgressId",
                table: "APIUserCourse");

            migrationBuilder.DropIndex(
                name: "IX_APIUserCourse_CourseProgressId",
                table: "APIUserCourse");

            migrationBuilder.DropColumn(
                name: "CourseProgressId",
                table: "APIUserCourse");

            migrationBuilder.AddColumn<long>(
                name: "UserCourseId",
                table: "CourseProgress",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgress_UserCourseId",
                table: "CourseProgress",
                column: "UserCourseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseProgress_APIUserCourse_UserCourseId",
                table: "CourseProgress",
                column: "UserCourseId",
                principalTable: "APIUserCourse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseProgress_APIUserCourse_UserCourseId",
                table: "CourseProgress");

            migrationBuilder.DropIndex(
                name: "IX_CourseProgress_UserCourseId",
                table: "CourseProgress");

            migrationBuilder.DropColumn(
                name: "UserCourseId",
                table: "CourseProgress");

            migrationBuilder.AddColumn<long>(
                name: "CourseProgressId",
                table: "APIUserCourse",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_APIUserCourse_CourseProgressId",
                table: "APIUserCourse",
                column: "CourseProgressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_APIUserCourse_CourseProgress_CourseProgressId",
                table: "APIUserCourse",
                column: "CourseProgressId",
                principalTable: "CourseProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
