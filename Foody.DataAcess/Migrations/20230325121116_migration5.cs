using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foody.DataAcess.Migrations
{
    public partial class migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CheckOuts_CheckoutId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOuts_Addresses_AddressId",
                table: "CheckOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CheckoutId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "CheckOuts",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckOuts_AddressId",
                table: "CheckOuts",
                newName: "IX_CheckOuts_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOuts_Orders_OrderId",
                table: "CheckOuts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOuts_Orders_OrderId",
                table: "CheckOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "CheckOuts",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckOuts_OrderId",
                table: "CheckOuts",
                newName: "IX_CheckOuts_AddressId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CheckoutId",
                table: "Addresses",
                column: "CheckoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_CheckOuts_CheckoutId",
                table: "Addresses",
                column: "CheckoutId",
                principalTable: "CheckOuts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOuts_Addresses_AddressId",
                table: "CheckOuts",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
