using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppView.Migrations
{
    /// <inheritdoc />
    public partial class tk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KhachHangId",
                table: "DatPhongs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaiKhoannID",
                table: "DatPhongs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatPhongs_TaiKhoannID",
                table: "DatPhongs",
                column: "TaiKhoannID");

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhongs_TaiKhoans_TaiKhoannID",
                table: "DatPhongs",
                column: "TaiKhoannID",
                principalTable: "TaiKhoans",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatPhongs_TaiKhoans_TaiKhoannID",
                table: "DatPhongs");

            migrationBuilder.DropIndex(
                name: "IX_DatPhongs_TaiKhoannID",
                table: "DatPhongs");

            migrationBuilder.DropColumn(
                name: "KhachHangId",
                table: "DatPhongs");

            migrationBuilder.DropColumn(
                name: "TaiKhoannID",
                table: "DatPhongs");
        }
    }
}
