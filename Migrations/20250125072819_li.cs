using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ice_cream.Migrations
{
    public partial class li : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "icecreamorder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "booksorder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "icecreamorder");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "booksorder");
        }
    }
}
