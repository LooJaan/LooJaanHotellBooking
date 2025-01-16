using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class Bookstoresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoaiPhongDichVu_LoaiPhongs_LoaiPhongsMaLoaiPhong",
                table: "LoaiPhongDichVu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiPhongDichVu",
                table: "LoaiPhongDichVu");

            migrationBuilder.DropIndex(
                name: "IX_LoaiPhongDichVu_LoaiPhongsMaLoaiPhong",
                table: "LoaiPhongDichVu");

            migrationBuilder.RenameColumn(
                name: "LoaiPhongsMaLoaiPhong",
                table: "LoaiPhongDichVu",
                newName: "ID");

            migrationBuilder.AddColumn<int>(
                name: "LoaiPhongsId",
                table: "LoaiPhongDichVu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiPhongDichVu",
                table: "LoaiPhongDichVu",
                columns: new[] { "DichVusID", "LoaiPhongsId" });

            migrationBuilder.CreateIndex(
                name: "IX_LoaiPhongDichVu_LoaiPhongsId",
                table: "LoaiPhongDichVu",
                column: "LoaiPhongsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoaiPhongDichVu_LoaiPhongs_LoaiPhongsId",
                table: "LoaiPhongDichVu",
                column: "LoaiPhongsId",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoaiPhongDichVu_LoaiPhongs_LoaiPhongsId",
                table: "LoaiPhongDichVu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiPhongDichVu",
                table: "LoaiPhongDichVu");

            migrationBuilder.DropIndex(
                name: "IX_LoaiPhongDichVu_LoaiPhongsId",
                table: "LoaiPhongDichVu");

            migrationBuilder.DropColumn(
                name: "LoaiPhongsId",
                table: "LoaiPhongDichVu");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "LoaiPhongDichVu",
                newName: "LoaiPhongsMaLoaiPhong");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiPhongDichVu",
                table: "LoaiPhongDichVu",
                columns: new[] { "DichVusID", "LoaiPhongsMaLoaiPhong" });

            migrationBuilder.CreateIndex(
                name: "IX_LoaiPhongDichVu_LoaiPhongsMaLoaiPhong",
                table: "LoaiPhongDichVu",
                column: "LoaiPhongsMaLoaiPhong");

            migrationBuilder.AddForeignKey(
                name: "FK_LoaiPhongDichVu_LoaiPhongs_LoaiPhongsMaLoaiPhong",
                table: "LoaiPhongDichVu",
                column: "LoaiPhongsMaLoaiPhong",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
