using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Members_TBL_LastLogIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogIn",
                table: "Members",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLogIn",
                table: "Members");
        }
    }
}
