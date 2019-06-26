using Microsoft.EntityFrameworkCore.Migrations;

namespace CosmoTrack.Migrations.CosmoTrackDb
{
    public partial class gh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFourURL",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ImageOneURL",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ImageThreeURL",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ImageTwoURL",
                table: "Reviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFourURL",
                table: "Reviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageOneURL",
                table: "Reviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageThreeURL",
                table: "Reviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageTwoURL",
                table: "Reviews",
                nullable: true);
        }
    }
}
