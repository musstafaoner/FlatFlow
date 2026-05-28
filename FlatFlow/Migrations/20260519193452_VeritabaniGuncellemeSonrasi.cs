using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatFlow.Migrations
{
    /// <inheritdoc />
    public partial class VeritabaniGuncellemeSonrasi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Odemeler",
                newName: "OdemeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Daireler",
                newName: "DaireId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ArizaTalepleri",
                newName: "ArizaTalepId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Aidatlar",
                newName: "AidatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OdemeId",
                table: "Odemeler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DaireId",
                table: "Daireler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ArizaTalepId",
                table: "ArizaTalepleri",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AidatId",
                table: "Aidatlar",
                newName: "Id");
        }
    }
}
