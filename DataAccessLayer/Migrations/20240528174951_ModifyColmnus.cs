using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aflamak.Migrations
{
    /// <inheritdoc />
    public partial class ModifyColmnus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Movies_SeriesId",
                table: "Episodes");

            migrationBuilder.RenameColumn(
                name: "SeriesId",
                table: "Episodes",
                newName: "TVShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Episodes_SeriesId",
                table: "Episodes",
                newName: "IX_Episodes_TVShowId");

            migrationBuilder.AddColumn<string>(
                name: "AnotherLangName",
                table: "Producers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnotherLangName",
                table: "Actors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Movies_TVShowId",
                table: "Episodes",
                column: "TVShowId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Movies_TVShowId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "AnotherLangName",
                table: "Producers");

            migrationBuilder.DropColumn(
                name: "AnotherLangName",
                table: "Actors");

            migrationBuilder.RenameColumn(
                name: "TVShowId",
                table: "Episodes",
                newName: "SeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Episodes_TVShowId",
                table: "Episodes",
                newName: "IX_Episodes_SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Movies_SeriesId",
                table: "Episodes",
                column: "SeriesId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
