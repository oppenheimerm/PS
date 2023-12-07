using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TBL_Station_AccesToliet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessibleToiletNearby",
                table: "PetrolStations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessibleToiletNearby",
                table: "PetrolStations");

            
        }
    }
}
