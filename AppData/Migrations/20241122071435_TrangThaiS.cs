using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class TrangThaiS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FB",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "GG",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "LogoFB",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "LogoGG",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "LogoTikTok",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "TikTok",
                table: "lienHes");

            migrationBuilder.DropColumn(
                name: "logoInstagram",
                table: "lienHes");

            migrationBuilder.CreateTable(
                name: "faceBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faceBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "gGs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gGs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tikTok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tikTok", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "faceBooks");

            migrationBuilder.DropTable(
                name: "gGs");

            migrationBuilder.DropTable(
                name: "tikTok");

            migrationBuilder.AddColumn<string>(
                name: "FB",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GG",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoFB",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoGG",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoTikTok",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TikTok",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "logoInstagram",
                table: "lienHes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
