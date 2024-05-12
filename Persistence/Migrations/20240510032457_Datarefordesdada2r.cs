using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Datarefordesdada2r : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParameterCheckErrorMessages",
                table: "ParameterCheckErrorMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ParameterCheckErrorMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "DataReferenceParameterChecks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParameterCheckErrorMessages",
                table: "ParameterCheckErrorMessages",
                columns: new[] { "Id", "ParameterCheckId", "ErrorMessageId" });

            migrationBuilder.CreateIndex(
                name: "IX_ParameterCheckErrorMessages_ParameterCheckId",
                table: "ParameterCheckErrorMessages",
                column: "ParameterCheckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ParameterCheckErrorMessages",
                table: "ParameterCheckErrorMessages");

            migrationBuilder.DropIndex(
                name: "IX_ParameterCheckErrorMessages_ParameterCheckId",
                table: "ParameterCheckErrorMessages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ParameterCheckErrorMessages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DataReferenceParameterChecks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParameterCheckErrorMessages",
                table: "ParameterCheckErrorMessages",
                columns: new[] { "ParameterCheckId", "ErrorMessageId" });
        }
    }
}
