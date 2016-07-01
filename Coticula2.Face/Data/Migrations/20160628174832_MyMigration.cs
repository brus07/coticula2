using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coticula2.Face.Data.Migrations
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    ProblemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.ProblemID);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguages",
                columns: table => new
                {
                    ProgrammingLanguageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguages", x => x.ProgrammingLanguageID);
                });

            migrationBuilder.CreateTable(
                name: "Verdicts",
                columns: table => new
                {
                    VerdictID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verdicts", x => x.VerdictID);
                });

            migrationBuilder.CreateTable(
                name: "Submits",
                columns: table => new
                {
                    SubmitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PeakMemoryUsed = table.Column<long>(nullable: false),
                    ProblemID = table.Column<int>(nullable: false),
                    ProgrammingLanguageID = table.Column<int>(nullable: false),
                    Solution = table.Column<string>(nullable: true),
                    SubmitTime = table.Column<DateTime>(nullable: false),
                    VerdictId = table.Column<int>(nullable: false),
                    WorkingTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submits", x => x.SubmitID);
                    table.ForeignKey(
                        name: "FK_Submits_Problems_ProblemID",
                        column: x => x.ProblemID,
                        principalTable: "Problems",
                        principalColumn: "ProblemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submits_ProgrammingLanguages_ProgrammingLanguageID",
                        column: x => x.ProgrammingLanguageID,
                        principalTable: "ProgrammingLanguages",
                        principalColumn: "ProgrammingLanguageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submits_Verdicts_VerdictId",
                        column: x => x.VerdictId,
                        principalTable: "Verdicts",
                        principalColumn: "VerdictID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submits_ProblemID",
                table: "Submits",
                column: "ProblemID");

            migrationBuilder.CreateIndex(
                name: "IX_Submits_ProgrammingLanguageID",
                table: "Submits",
                column: "ProgrammingLanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Submits_VerdictId",
                table: "Submits",
                column: "VerdictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submits");

            migrationBuilder.DropTable(
                name: "Problems");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguages");

            migrationBuilder.DropTable(
                name: "Verdicts");
        }
    }
}
