using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aflamak.Migrations
{
    /// <inheritdoc />
    public partial class FixDataBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EPISODES WHERE PARTID IN (76,77,78,79,80,81)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO EPISODES SELECT EPISODENO , PARTID FROM EPISDOES WHERE PARTID IN (76,77,78,79,80,81)");
        }
    }
}
