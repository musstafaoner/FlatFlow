using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatFlow.Migrations
{
    /// <inheritdoc />
    public partial class CokluSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Duyurular",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Daireler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "ArizaTalepleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Aidatlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Siteler",
                columns: table => new
                {
                    SiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siteler", x => x.SiteId);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciSiteler",
                columns: table => new
                {
                    KullaniciSiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciSiteler", x => x.KullaniciSiteId);
                    table.ForeignKey(
                        name: "FK_KullaniciSiteler_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciSiteler_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_SiteId",
                table: "Duyurular",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Daireler_SiteId",
                table: "Daireler",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ArizaTalepleri_SiteId",
                table: "ArizaTalepleri",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Aidatlar_SiteId",
                table: "Aidatlar",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciSiteler_KullaniciId",
                table: "KullaniciSiteler",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciSiteler_SiteId",
                table: "KullaniciSiteler",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aidatlar_Siteler_SiteId",
                table: "Aidatlar",
                column: "SiteId",
                principalTable: "Siteler",
                principalColumn: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArizaTalepleri_Siteler_SiteId",
                table: "ArizaTalepleri",
                column: "SiteId",
                principalTable: "Siteler",
                principalColumn: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Daireler_Siteler_SiteId",
                table: "Daireler",
                column: "SiteId",
                principalTable: "Siteler",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Duyurular_Siteler_SiteId",
                table: "Duyurular",
                column: "SiteId",
                principalTable: "Siteler",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aidatlar_Siteler_SiteId",
                table: "Aidatlar");

            migrationBuilder.DropForeignKey(
                name: "FK_ArizaTalepleri_Siteler_SiteId",
                table: "ArizaTalepleri");

            migrationBuilder.DropForeignKey(
                name: "FK_Daireler_Siteler_SiteId",
                table: "Daireler");

            migrationBuilder.DropForeignKey(
                name: "FK_Duyurular_Siteler_SiteId",
                table: "Duyurular");

            migrationBuilder.DropTable(
                name: "KullaniciSiteler");

            migrationBuilder.DropTable(
                name: "Siteler");

            migrationBuilder.DropIndex(
                name: "IX_Duyurular_SiteId",
                table: "Duyurular");

            migrationBuilder.DropIndex(
                name: "IX_Daireler_SiteId",
                table: "Daireler");

            migrationBuilder.DropIndex(
                name: "IX_ArizaTalepleri_SiteId",
                table: "ArizaTalepleri");

            migrationBuilder.DropIndex(
                name: "IX_Aidatlar_SiteId",
                table: "Aidatlar");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Duyurular");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Daireler");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "ArizaTalepleri");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Aidatlar");
        }
    }
}
