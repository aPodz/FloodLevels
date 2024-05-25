using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloodLevels.Data.Migrations
{
    /// <inheritdoc />
    public partial class Values : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                          name: "Values",
                          columns: table => new
                          {
                              Id = table.Column<int>(type: "int", nullable: false)
                                  .Annotation("SqlServer:Identity", "1, 1"),
                              Value = table.Column<int>(type: "int", nullable: false),
                              Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                              StationId = table.Column<int>(type: "int", nullable: false)
                          },
                          constraints: table =>
                          {
                              table.PrimaryKey("PK_Values", x => x.Id);
                              table.ForeignKey(
                                  name: "FK_Values_Stations_StationId",
                                  column: x => x.StationId,
                                  principalTable: "Stations",
                                  principalColumn: "Id",
                                  onDelete: ReferentialAction.Cascade);
                          });

            migrationBuilder.CreateIndex(
                name: "IX_Values_StationId",
                table: "Values",
                column: "StationId");      
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Values");
        }
    }
}
