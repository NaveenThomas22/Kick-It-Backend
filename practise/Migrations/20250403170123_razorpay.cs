using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practise.Migrations
{
    /// <inheritdoc />
    public partial class razorpay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "Products",
                newName: "Image");
        }
    }
}
