using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    public partial class AddSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportIssueType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PublicId = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportIssueType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportIssue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PublicId = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    IssueTypeId = table.Column<long>(type: "bigint", nullable: false),
                    ReporterId = table.Column<string>(type: "text", nullable: false),
                    IssueEntityId = table.Column<string>(type: "text", nullable: true),
                    IssueMessage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportIssue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportIssue_IssueTypeId",
                        column: x => x.IssueTypeId,
                        principalTable: "SupportIssueType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportIssue_IssueTypeId",
                table: "SupportIssue",
                column: "IssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportIssue_PublicId",
                table: "SupportIssue",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportIssueType_PublicId",
                table: "SupportIssueType",
                column: "PublicId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportIssue");

            migrationBuilder.DropTable(
                name: "SupportIssueType");
        }
    }
}
