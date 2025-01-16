using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class hello : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatPhongs_PhongChiTiets_PhongChiTietID",
                table: "DatPhongs");

            migrationBuilder.DropTable(
                name: "PhongChiTiets");

            migrationBuilder.DropColumn(
                name: "IDPhongChiTiet",
                table: "DatPhongs");

            migrationBuilder.RenameColumn(
                name: "PhongChiTietID",
                table: "DatPhongs",
                newName: "LoaiPhongID");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhongs_PhongChiTietID",
                table: "DatPhongs",
                newName: "IX_DatPhongs_LoaiPhongID");

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhongs_LoaiPhongs_LoaiPhongID",
                table: "DatPhongs",
                column: "LoaiPhongID",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatPhongs_LoaiPhongs_LoaiPhongID",
                table: "DatPhongs");

            migrationBuilder.RenameColumn(
                name: "LoaiPhongID",
                table: "DatPhongs",
                newName: "PhongChiTietID");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhongs_LoaiPhongID",
                table: "DatPhongs",
                newName: "IX_DatPhongs_PhongChiTietID");

            migrationBuilder.AddColumn<int>(
                name: "IDPhongChiTiet",
                table: "DatPhongs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhongChiTiets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiPhongMaLoaiPhong = table.Column<int>(type: "int", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoNguoi = table.Column<int>(type: "int", nullable: false),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongChiTiets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhongChiTiets_LoaiPhongs_LoaiPhongMaLoaiPhong",
                        column: x => x.LoaiPhongMaLoaiPhong,
                        principalTable: "LoaiPhongs",
                        principalColumn: "MaLoaiPhong");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhongChiTiets_LoaiPhongMaLoaiPhong",
                table: "PhongChiTiets",
                column: "LoaiPhongMaLoaiPhong");

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhongs_PhongChiTiets_PhongChiTietID",
                table: "DatPhongs",
                column: "PhongChiTietID",
                principalTable: "PhongChiTiets",
                principalColumn: "ID");
        }
    }
}
