using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TIPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class idinmembershare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MemberShares",
                newName: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "MemberShares",
                newName: "Name");
        }
    }
}
