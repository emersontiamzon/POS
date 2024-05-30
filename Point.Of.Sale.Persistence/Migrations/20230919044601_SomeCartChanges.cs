using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Point.Of.Sale.Persistence.Models;

#nullable disable

namespace Point.Of.Sale.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SomeCartChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<List<ShoppingCartLineItem>>(
                name: "LineItems",
                table: "ShoppingCarts",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineItems",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShoppingCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
