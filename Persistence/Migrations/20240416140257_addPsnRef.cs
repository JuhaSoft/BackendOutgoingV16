using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class addPsnRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PsnPos",
                table: "DataReferences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefCompare",
                table: "DataReferences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefPos",
                table: "DataReferences",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PsnPos",
                table: "DataReferences");

            migrationBuilder.DropColumn(
                name: "RefCompare",
                table: "DataReferences");

            migrationBuilder.DropColumn(
                name: "RefPos",
                table: "DataReferences");
        }
    }
}
