using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HighscoreAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HighscoreLists",
                columns: table => new
                {
                    HighScoreId = table.Column<Guid>(nullable: false),
                    User = table.Column<string>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighscoreLists", x => x.HighScoreId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HighscoreLists");
        }
    }
}
