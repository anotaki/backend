﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anotaki_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalesQuantity",
                table: "Products",
                newName: "SalesCount");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SalesCount",
                table: "Products",
                newName: "SalesQuantity");
        }
    }
}
