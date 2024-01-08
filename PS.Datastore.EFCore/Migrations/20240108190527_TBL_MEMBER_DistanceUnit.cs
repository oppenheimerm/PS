using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TBL_MEMBER_DistanceUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistanceUnit",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceUnit",
                table: "Members");
        }
    }
}
