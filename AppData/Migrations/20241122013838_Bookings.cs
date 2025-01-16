using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class Bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_LoaiPhongs_LoaiPhongId",
                table: "DichVus");

            migrationBuilder.DropIndex(
                name: "IX_DichVus_LoaiPhongId",
                table: "DichVus");

            migrationBuilder.CreateTable(
                name: "LoaiPhongDichVu",
                columns: table => new
                {
                    DichVusID = table.Column<int>(type: "int", nullable: false),
                    LoaiPhongsMaLoaiPhong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhongDichVu", x => new { x.DichVusID, x.LoaiPhongsMaLoaiPhong });
                    table.ForeignKey(
                        name: "FK_LoaiPhongDichVu_DichVus_DichVusID",
                        column: x => x.DichVusID,
                        principalTable: "DichVus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoaiPhongDichVu_LoaiPhongs_LoaiPhongsMaLoaiPhong",
                        column: x => x.LoaiPhongsMaLoaiPhong,
                        principalTable: "LoaiPhongs",
                        principalColumn: "MaLoaiPhong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoaiPhongDichVu_LoaiPhongsMaLoaiPhong",
                table: "LoaiPhongDichVu",
                column: "LoaiPhongsMaLoaiPhong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoaiPhongDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_LoaiPhongId",
                table: "DichVus",
                column: "LoaiPhongId");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVus_LoaiPhongs_LoaiPhongId",
                table: "DichVus",
                column: "LoaiPhongId",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
