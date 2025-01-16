using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class updat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "TenTinTuc",
                table: "tinTucs");

            migrationBuilder.RenameColumn(
                name: "NoiDungChiTiet",
                table: "tinTucs",
                newName: "TenTinTucPhu");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh1",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh2",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh3",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet1",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet2",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet3",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungChiTiet4",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenTinTucChinh",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh1",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "HinhAnh2",
                table: "tinTucs");

            migrationBuilder.DropColumn(
                name: "HinhAnh3",
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
                name: "TenTinTucChinh",
                table: "tinTucs");

            migrationBuilder.RenameColumn(
                name: "TenTinTucPhu",
                table: "tinTucs",
                newName: "NoiDungChiTiet");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenTinTuc",
                table: "tinTucs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
