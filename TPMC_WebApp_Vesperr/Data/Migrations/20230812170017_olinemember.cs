using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class olinemember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Cart");
        }
    }
}
