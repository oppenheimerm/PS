using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PS.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class tbl_vendor_vendorlogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorLogo",
                table: "PetrolVendors",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorLogo",
                table: "PetrolVendors");
        }
    }
}
