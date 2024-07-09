using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aflamak.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_TvShows_TvShowId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Part",
                table: "TvShows");

            migrationBuilder.RenameColumn(
                name: "TvShowId",
                table: "Episodes",
                newName: "PartId");

            migrationBuilder.RenameIndex(
                name: "IX_Episodes_TvShowId",
                table: "Episodes",
                newName: "IX_Episodes_PartId");

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EpisodesNo = table.Column<int>(type: "int", nullable: false),
                    NoOfLikes = table.Column<int>(type: "int", nullable: true),
                    TvShowId = table.Column<int>(type: "int", nullable: false),
                    dbImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_TvShows_TvShowId",
                        column: x => x.TvShowId,
                        principalTable: "TvShows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parts_TvShowId",
                table: "Parts",
                column: "TvShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Parts_PartId",
                table: "Episodes",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Parts_PartId",
                table: "Episodes");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.RenameColumn(
                name: "PartId",
                table: "Episodes",
                newName: "TvShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Episodes_PartId",
                table: "Episodes",
                newName: "IX_Episodes_TvShowId");

            migrationBuilder.AddColumn<int>(
                name: "Part",
                table: "TvShows",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_TvShows_TvShowId",
                table: "Episodes",
                column: "TvShowId",
                principalTable: "TvShows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
