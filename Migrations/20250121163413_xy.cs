using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ice_cream.Migrations
{
    public partial class xy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Booktitle",
                table: "booksorder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Booktitle",
                table: "booksorder");
        }
    }
}
