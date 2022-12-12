using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class AddCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PublicId = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LanguageFromId = table.Column<long>(type: "bigint", nullable: false),
                    LanguageToId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Languages_LanguageFromId",
                        column: x => x.LanguageFromId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Courses_Languages_LanguageToId",
                        column: x => x.LanguageToId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LanguageFromId",
                table: "Courses",
                column: "LanguageFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LanguageToId",
                table: "Courses",
                column: "LanguageToId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PublicId",
                table: "Courses",
                column: "PublicId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
