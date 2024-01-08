using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TBL_MEMBER_MOBILE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Members");
        }
    }
}
