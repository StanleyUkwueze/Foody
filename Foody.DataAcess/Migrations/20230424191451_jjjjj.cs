using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foody.DataAcess.Migrations
{
    public partial class jjjjj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOuts_Orders_OrderId",
                table: "CheckOuts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CheckOutId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_CheckOuts_OrderId",
                table: "CheckOuts");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CheckOutId",
                table: "Orders",
                column: "CheckOutId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CheckOutId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CheckOutId",
                table: "Orders",
                column: "CheckOutId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_OrderId",
                table: "CheckOuts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOuts_Orders_OrderId",
                table: "CheckOuts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
