using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ice_cream.Migrations
{
    public partial class ab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "icecreams",
                columns: table => new
                {
                    IcecreamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icecreamname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icecreamprice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_icecreams", x => x.IcecreamId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "icecreams");
        }
    }
}
