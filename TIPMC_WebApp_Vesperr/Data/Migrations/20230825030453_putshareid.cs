﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TIPMC_WebApp_Vesperr.Data.Migrations
{
    public partial class putshareid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShareId",
                table: "MemberShares",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareId",
                table: "MemberShares");
        }
    }
}
