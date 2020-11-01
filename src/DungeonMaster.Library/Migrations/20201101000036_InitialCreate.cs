using Microsoft.EntityFrameworkCore.Migrations;

namespace DungeonMaster.Library.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuildMasters",
                columns: table => new
                {
                    Guild = table.Column<ulong>(nullable: false),
                    AdminitrationCategoryChannel = table.Column<ulong>(nullable: true),
                    AuditLogChannel = table.Column<ulong>(nullable: true),
                    TogglePowerChannel = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMasters", x => x.Guild);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildMasters");
        }
    }
}
