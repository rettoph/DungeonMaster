using Microsoft.EntityFrameworkCore.Metadata;
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guild = table.Column<ulong>(nullable: true),
                    AdminitrationCategoryChannel = table.Column<ulong>(nullable: true),
                    AuditLogChannel = table.Column<ulong>(nullable: true),
                    TogglePowerChannel = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageReferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<ulong>(nullable: false),
                    ChannelId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReactMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MessageReferenceId = table.Column<int>(nullable: true),
                    GuildMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactMenus_GuildMasters_GuildMasterId",
                        column: x => x.GuildMasterId,
                        principalTable: "GuildMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReactMenus_MessageReferences_MessageReferenceId",
                        column: x => x.MessageReferenceId,
                        principalTable: "MessageReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReactMenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReactMenuId = table.Column<int>(nullable: true),
                    Emote = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactMenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactMenuItems_ReactMenus_ReactMenuId",
                        column: x => x.ReactMenuId,
                        principalTable: "ReactMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildMasters_Guild",
                table: "GuildMasters",
                column: "Guild");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReferences_ChannelId_MessageId",
                table: "MessageReferences",
                columns: new[] { "ChannelId", "MessageId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReactMenuItems_ReactMenuId",
                table: "ReactMenuItems",
                column: "ReactMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactMenus_GuildMasterId",
                table: "ReactMenus",
                column: "GuildMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactMenus_MessageReferenceId",
                table: "ReactMenus",
                column: "MessageReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactMenuItems");

            migrationBuilder.DropTable(
                name: "ReactMenus");

            migrationBuilder.DropTable(
                name: "GuildMasters");

            migrationBuilder.DropTable(
                name: "MessageReferences");
        }
    }
}
