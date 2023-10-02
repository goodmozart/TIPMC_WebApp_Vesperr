using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TIPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class putshareidkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "MemberShares",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShares",
                table: "MemberShares",
                column: "ShareId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
               name: "MemberId",
               table: "MemberShares",
               type: "nvarchar(max)",
               nullable: true,
               oldClrType: typeof(string),
               oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShares",
                table: "MemberShares",
                column: "ShareId");
        }
    }
}
