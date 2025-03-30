using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankomizer.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_songs",
                table: "songs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_paintings",
                table: "paintings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_movies",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "paintings");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "paintings");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Movies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_songs",
                table: "songs",
                column: "item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_paintings",
                table: "paintings",
                column: "item_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "item_id");

            migrationBuilder.CreateTable(
                name: "catalog_entry",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_entry", x => x.item_id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_movies_catalog_entry_item_id",
                table: "Movies",
                column: "item_id",
                principalTable: "catalog_entry",
                principalColumn: "item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_paintings_catalog_entry_item_id",
                table: "paintings",
                column: "item_id",
                principalTable: "catalog_entry",
                principalColumn: "item_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_songs_catalog_entry_item_id",
                table: "songs",
                column: "item_id",
                principalTable: "catalog_entry",
                principalColumn: "item_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_movies_catalog_entry_item_id",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "fk_paintings_catalog_entry_item_id",
                table: "paintings");

            migrationBuilder.DropForeignKey(
                name: "fk_songs_catalog_entry_item_id",
                table: "songs");

            migrationBuilder.DropTable(
                name: "catalog_entry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_songs",
                table: "songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_paintings",
                table: "paintings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "songs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "songs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "paintings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "paintings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Movies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Movies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "pk_songs",
                table: "songs",
                column: "item_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_paintings",
                table: "paintings",
                column: "item_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_movies",
                table: "Movies",
                column: "item_id");
        }
    }
}
