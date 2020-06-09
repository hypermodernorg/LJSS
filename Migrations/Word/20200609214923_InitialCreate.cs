using Microsoft.EntityFrameworkCore.Migrations;

namespace LJSS.Migrations.Word
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordModel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    English = table.Column<string>(type: "TEXT", nullable: true),
                    Japanese = table.Column<string>(type: "TEXT", nullable: true),
                    Pronunciation = table.Column<string>(type: "TEXT", nullable: true),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Synonyms = table.Column<string>(type: "TEXT", nullable: true),
                    Example = table.Column<string>(type: "TEXT", nullable: true),
                    System = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordModel", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordModel");
        }
    }
}
