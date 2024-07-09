using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aflamak.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnInModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfDisLikes",
                table: "TvShows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfDisLikes",
                table: "Films",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfDisLikes",
                table: "TvShows");

            migrationBuilder.DropColumn(
                name: "NoOfDisLikes",
                table: "Films");
        }
    }
}
