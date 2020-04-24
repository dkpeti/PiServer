using Microsoft.EntityFrameworkCore.Migrations;

namespace PiServer.Migrations
{
    public partial class PiServerPiDbContextInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Szenzorok",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    Tipus = table.Column<int>(nullable: false),
                    RemoteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Szenzorok", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Szenzorok");
        }
    }
}
