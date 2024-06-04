using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class emailconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebConfigDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailRegisterTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailRegisterBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailInfoTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailInfoBody = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebConfigDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebConfigDatas");
        }
    }
}
