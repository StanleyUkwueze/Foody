using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foody.DataAcess.Migrations
{
    public partial class checkoutremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CheckOuts_CheckOutId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CheckOuts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CheckOutId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CheckOutId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "CheckOutId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CheckOuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOuts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CheckOutId",
                table: "Orders",
                column: "CheckOutId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CheckOuts_CheckOutId",
                table: "Orders",
                column: "CheckOutId",
                principalTable: "CheckOuts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
