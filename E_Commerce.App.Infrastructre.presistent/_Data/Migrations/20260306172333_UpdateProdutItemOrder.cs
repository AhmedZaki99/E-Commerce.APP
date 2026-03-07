using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.App.Infrastructre.presistent._Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProdutItemOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Product_Vendor",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product_Vendor",
                table: "OrderItems");
        }
    }
}
