using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class CoursesDoneandDataDoneadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseProgressDataDone",
                columns: table => new
                {
                    CourseProgressId = table.Column<long>(type: "bigint", nullable: false),
                    CourseDataId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProgressDataDone", x => new { x.CourseProgressId, x.CourseDataId });
                    table.ForeignKey(
                        name: "FK_CourseProgressDataDone_CourseData_CourseDataId",
                        column: x => x.CourseDataId,
                        principalTable: "CourseData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProgressDataDone_CourseProgress_CourseProgressId",
                        column: x => x.CourseProgressId,
                        principalTable: "CourseProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursesDone",
                columns: table => new
                {
                    APIUserId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesDone", x => new { x.APIUserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CoursesDone_AspNetUsers_APIUserId",
                        column: x => x.APIUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursesDone_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgressDataDone_CourseDataId",
                table: "CourseProgressDataDone",
                column: "CourseDataId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesDone_CourseId",
                table: "CoursesDone",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseProgressDataDone");

            migrationBuilder.DropTable(
                name: "CoursesDone");
        }
    }
}
