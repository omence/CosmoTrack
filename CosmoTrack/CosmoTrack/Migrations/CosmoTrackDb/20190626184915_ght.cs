using Microsoft.EntityFrameworkCore.Migrations;

namespace CosmoTrack.Migrations.CosmoTrackDb
{
    public partial class ght : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "UserJournals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "UserJournals");
        }
    }
}
