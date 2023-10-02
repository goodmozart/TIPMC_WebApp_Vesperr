using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class checkout2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemberId",
                table: "OrderItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "OrderItem");
        }
    }
}
