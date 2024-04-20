using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class EditparameterCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataTrackCheckings_ParameterChecks_PCID",
                table: "DataTrackCheckings");

            migrationBuilder.DropIndex(
                name: "IX_DataTrackCheckings_PCID",
                table: "DataTrackCheckings");

            migrationBuilder.DropColumn(
                name: "PCDateCreate",
                table: "ParameterChecks");

            migrationBuilder.DropColumn(
                name: "PCDescription",
                table: "ParameterChecks");

            migrationBuilder.DropColumn(
                name: "PCName",
                table: "ParameterChecks");

            migrationBuilder.DropColumn(
                name: "PCisDeleted",
                table: "ParameterChecks");

            migrationBuilder.RenameColumn(
                name: "PCUserCreate",
                table: "ParameterChecks",
                newName: "Description");

            migrationBuilder.AddColumn<Guid>(
                name: "DataReferenceId",
                table: "ParameterChecks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ParameterChecks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ParameterChecksId",
                table: "DataTrackCheckings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterChecks_DataReferenceId",
                table: "ParameterChecks",
                column: "DataReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTrackCheckings_ParameterChecksId",
                table: "DataTrackCheckings",
                column: "ParameterChecksId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataTrackCheckings_ParameterChecks_ParameterChecksId",
                table: "DataTrackCheckings",
                column: "ParameterChecksId",
                principalTable: "ParameterChecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterChecks_DataReferences_DataReferenceId",
                table: "ParameterChecks",
                column: "DataReferenceId",
                principalTable: "DataReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataTrackCheckings_ParameterChecks_ParameterChecksId",
                table: "DataTrackCheckings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterChecks_DataReferences_DataReferenceId",
                table: "ParameterChecks");

            migrationBuilder.DropIndex(
                name: "IX_ParameterChecks_DataReferenceId",
                table: "ParameterChecks");

            migrationBuilder.DropIndex(
                name: "IX_DataTrackCheckings_ParameterChecksId",
                table: "DataTrackCheckings");

            migrationBuilder.DropColumn(
                name: "DataReferenceId",
                table: "ParameterChecks");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ParameterChecks");

            migrationBuilder.DropColumn(
                name: "ParameterChecksId",
                table: "DataTrackCheckings");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ParameterChecks",
                newName: "PCUserCreate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PCDateCreate",
                table: "ParameterChecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PCDescription",
                table: "ParameterChecks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PCName",
                table: "ParameterChecks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PCisDeleted",
                table: "ParameterChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_DataTrackCheckings_PCID",
                table: "DataTrackCheckings",
                column: "PCID");

            migrationBuilder.AddForeignKey(
                name: "FK_DataTrackCheckings_ParameterChecks_PCID",
                table: "DataTrackCheckings",
                column: "PCID",
                principalTable: "ParameterChecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
