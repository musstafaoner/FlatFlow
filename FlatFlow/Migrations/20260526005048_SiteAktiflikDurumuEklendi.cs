using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatFlow.Migrations
{
    /// <inheritdoc />
    public partial class SiteAktiflikDurumuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "Siteler",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "Siteler");
        }
    }
}
