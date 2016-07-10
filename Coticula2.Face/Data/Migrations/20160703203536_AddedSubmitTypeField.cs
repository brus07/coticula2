using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coticula2.Face.Data.Migrations
{
    public partial class AddedSubmitTypeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmitTypeId",
                table: "Submits",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Submits_SubmitTypeId",
                table: "Submits",
                column: "SubmitTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submits_SubmitTypes_SubmitTypeId",
                table: "Submits",
                column: "SubmitTypeId",
                principalTable: "SubmitTypes",
                principalColumn: "SubmitTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submits_SubmitTypes_SubmitTypeId",
                table: "Submits");

            migrationBuilder.DropIndex(
                name: "IX_Submits_SubmitTypeId",
                table: "Submits");

            migrationBuilder.DropColumn(
                name: "SubmitTypeId",
                table: "Submits");
        }
    }
}
