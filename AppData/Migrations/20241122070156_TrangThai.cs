using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class TrangThai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlFb",
                table: "lienHes");

            migrationBuilder.RenameColumn(
                name: "UrlTikTok",
                table: "lienHes",
                newName: "LogoGG");

            migrationBuilder.RenameColumn(
                name: "UrlInstagram",
                table: "lienHes",
                newName: "GG");

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "welComes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "tinTucs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "Slides",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "PhongChiTiets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "loGos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "LoaiPhongs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "lienHes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "DichVus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "AnhChiTiets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "welComes");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "PhongChiTiets");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "loGos");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "LoaiPhongs");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "AnhChiTiets");

            migrationBuilder.RenameColumn(
                name: "LogoGG",
                table: "lienHes",
                newName: "UrlTikTok");

            migrationBuilder.RenameColumn(
                name: "GG",
                table: "lienHes",
                newName: "UrlInstagram");

            migrationBuilder.AddColumn<string>(
                name: "UrlFb",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
