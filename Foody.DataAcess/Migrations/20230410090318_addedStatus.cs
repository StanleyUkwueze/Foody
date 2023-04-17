using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foody.DataAcess.Migrations
{
    public partial class addedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShoppingCarts");
        }
    }
}
