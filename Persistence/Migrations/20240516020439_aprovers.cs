using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class aprovers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Appove",
                table: "DataTrackCheckings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApprRemaks",
                table: "DataTrackCheckings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appove",
                table: "DataTrackCheckings");

            migrationBuilder.DropColumn(
                name: "ApprRemaks",
                table: "DataTrackCheckings");
        }
    }
}
