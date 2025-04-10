﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePreformedByColumnOfFollowUpsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowUpDate",
                table: "OrderFollowUps");

            migrationBuilder.DropColumn(
                name: "PerformedBy",
                table: "OrderFollowUps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpDate",
                table: "OrderFollowUps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerformedBy",
                table: "OrderFollowUps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
