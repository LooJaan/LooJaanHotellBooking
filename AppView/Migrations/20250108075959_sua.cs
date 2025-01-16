using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppView.Migrations
{
    /// <inheritdoc />
    public partial class sua : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Xoa",
                table: "DatPhongs");

            migrationBuilder.AddColumn<bool>(
                name: "Xoa",
                table: "LoaiPhongs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Xoa",
                table: "LoaiPhongs");

            migrationBuilder.AddColumn<bool>(
                name: "Xoa",
                table: "DatPhongs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
