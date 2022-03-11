using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Filters.Demo.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Manufacturer", "Model", "Type" },
                values: new object[,]
                {
                    { 1, "YT", "Izzo", 3 },
                    { 2, "YT", "Jeffsy", 3 },
                    { 3, "YT", "Capra", 4 },
                    { 4, "YT", "Tues", 5 },
                    { 5, "Propain", "Hugene", 3 },
                    { 6, "Propain", "Tyee", 4 },
                    { 7, "Propain", "Spindrift", 4 },
                    { 8, "Propain", "Rage", 5 },
                    { 9, "Canyon", "Aero", 0 },
                    { 10, "Canyon", "Endurance", 0 },
                    { 11, "Canyon", "Race", 0 },
                    { 12, "Canyon", "Grizl", 1 },
                    { 13, "Canyon", "Grail", 1 },
                    { 14, "Canyon", "Exceed", 2 },
                    { 15, "Canyon", "Lux", 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bikes");
        }
    }
}
