using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppView.Migrations
{
    /// <inheritdoc />
    public partial class luansua : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh1",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "HinhAnh2",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "NoiDungChiTiet1",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "NoiDungChiTiet2",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "NoiDungChiTiet3",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "NoiDungChiTiet4",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "TenTinTucPhu",
                table: "tinTucs");

            migrationBuilder.RenameColumn(
                name: "HinhAnh3",
                table: "tinTucs",
                newName: "HinhAnh");

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoiDungChiTiet",
                table: "tinTucs");

            migrationBuilder.RenameColumn(
                name: "HinhAnh",
                table: "tinTucs",
                newName: "HinhAnh3");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh1",
                table: "tinTucs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh2",
                table: "tinTucs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet1",
                table: "tinTucs",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet2",
                table: "tinTucs",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet3",
                table: "tinTucs",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet4",
                table: "tinTucs",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenTinTucPhu",
                table: "tinTucs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
