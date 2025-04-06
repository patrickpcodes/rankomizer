using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankomizer.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedGauntlet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gauntlets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gauntlets", x => x.id);
                    table.ForeignKey(
                        name: "fk_gauntlets_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gauntlets_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "Collections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RosterItems",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    gauntlet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    wins = table.Column<int>(type: "integer", nullable: false),
                    losses = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roster_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_roster_items_gauntlets_gauntlet_id",
                        column: x => x.gauntlet_id,
                        principalTable: "Gauntlets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_roster_items_items_item_id",
                        column: x => x.item_id,
                        principalTable: "Items",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Duels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    gauntlet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    roster_item_a_id = table.Column<Guid>(type: "uuid", nullable: false),
                    roster_item_b_id = table.Column<Guid>(type: "uuid", nullable: false),
                    winner_roster_item_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_duels", x => x.id);
                    table.ForeignKey(
                        name: "fk_duels_gauntlets_gauntlet_id",
                        column: x => x.gauntlet_id,
                        principalTable: "Gauntlets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_duels_roster_items_roster_item_a_id",
                        column: x => x.roster_item_a_id,
                        principalTable: "RosterItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_duels_roster_items_roster_item_b_id",
                        column: x => x.roster_item_b_id,
                        principalTable: "RosterItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_duels_roster_items_winner_roster_item_id",
                        column: x => x.winner_roster_item_id,
                        principalTable: "RosterItems",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_duels_gauntlet_id",
                table: "Duels",
                column: "gauntlet_id");

            migrationBuilder.CreateIndex(
                name: "ix_duels_roster_item_a_id",
                table: "Duels",
                column: "roster_item_a_id");

            migrationBuilder.CreateIndex(
                name: "ix_duels_roster_item_b_id",
                table: "Duels",
                column: "roster_item_b_id");

            migrationBuilder.CreateIndex(
                name: "ix_duels_winner_roster_item_id",
                table: "Duels",
                column: "winner_roster_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_gauntlets_collection_id",
                table: "Gauntlets",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_gauntlets_user_id",
                table: "Gauntlets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_roster_items_gauntlet_id",
                table: "RosterItems",
                column: "gauntlet_id");

            migrationBuilder.CreateIndex(
                name: "ix_roster_items_item_id",
                table: "RosterItems",
                column: "item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duels");

            migrationBuilder.DropTable(
                name: "RosterItems");

            migrationBuilder.DropTable(
                name: "Gauntlets");
        }
    }
}
