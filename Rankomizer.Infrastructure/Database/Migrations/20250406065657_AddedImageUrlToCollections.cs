using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankomizer.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageUrlToCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "Collections",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "Collections");
        }
    }
}
