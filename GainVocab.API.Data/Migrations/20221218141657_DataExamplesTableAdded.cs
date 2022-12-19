using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class DataExamplesTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseDataExample",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PublicId = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Translation = table.Column<string>(type: "text", nullable: false),
                    CourseDataId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDataExample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDataExample_CourseDataId",
                        column: x => x.CourseDataId,
                        principalTable: "CourseData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDataExample_CourseDataId",
                table: "CourseDataExample",
                column: "CourseDataId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDataExample_PublicId",
                table: "CourseDataExample",
                column: "PublicId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDataExample");
        }
    }
}
