using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class riki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "SalesOrderLine",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "SalesOrderLine");
        }
    }
}
