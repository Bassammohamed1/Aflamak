using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aflamak.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnToMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Part",
                table: "Movies",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Part",
                table: "Movies");
        }
    }
}
