using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class Bookstore3o : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnhChiTiets_LoaiPhongs_IdLoaiPhong",
                table: "AnhChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_AnhChiTiets_IdLoaiPhong",
                table: "AnhChiTiets");

            migrationBuilder.AddColumn<int>(
                name: "LoaiPhongMaLoaiPhong",
                table: "AnhChiTiets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnhChiTiets_LoaiPhongMaLoaiPhong",
                table: "AnhChiTiets",
                column: "LoaiPhongMaLoaiPhong");

            migrationBuilder.AddForeignKey(
                name: "FK_AnhChiTiets_LoaiPhongs_LoaiPhongMaLoaiPhong",
                table: "AnhChiTiets",
                column: "LoaiPhongMaLoaiPhong",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnhChiTiets_LoaiPhongs_LoaiPhongMaLoaiPhong",
                table: "AnhChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_AnhChiTiets_LoaiPhongMaLoaiPhong",
                table: "AnhChiTiets");

            migrationBuilder.DropColumn(
                name: "LoaiPhongMaLoaiPhong",
                table: "AnhChiTiets");

            migrationBuilder.CreateIndex(
                name: "IX_AnhChiTiets_IdLoaiPhong",
                table: "AnhChiTiets",
                column: "IdLoaiPhong");

            migrationBuilder.AddForeignKey(
                name: "FK_AnhChiTiets_LoaiPhongs_IdLoaiPhong",
                table: "AnhChiTiets",
                column: "IdLoaiPhong",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
