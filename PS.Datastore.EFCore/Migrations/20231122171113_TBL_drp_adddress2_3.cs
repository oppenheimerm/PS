using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TBL_drp_adddress2_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorAddress2",
                table: "PetrolVendors");

            migrationBuilder.DropColumn(
                name: "VendorAddress3",
                table: "PetrolVendors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorAddress2",
                table: "PetrolVendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorAddress3",
                table: "PetrolVendors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
