using Microsoft.EntityFrameworkCore.Migrations;

namespace TPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class ExtendASPNetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarURL",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "FullName",
               table: "AspNetUsers",
               nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "EmpNumber",
               table: "AspNetUsers",
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarURL",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
               name: "FullName",
               table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
               name: "EmpNumber",
               table: "AspNetUsers");
        }
    }
}
