using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TemuFlix.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Director = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    PriceUSD = table.Column<decimal>(type: "TEXT", nullable: false),
                    PosterUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Genre", "PosterUrl", "PriceUSD", "Rating", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Złodziej kradnący sekrety poprzez technologię wspólnych snów.", "Christopher Nolan", "Sci-Fi", "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_SX300.jpg", 9.99m, 8.8000000000000007, "Inception", 2010 },
                    { 2, "Haker odkrywa, że rzeczywistość jest symulacją.", "The Wachowskis", "Action", "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVlLTM5YTUtZjk4OGU3ODUxNzgxXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg", 7.99m, 8.6999999999999993, "The Matrix", 1999 },
                    { 3, "Eksploratorzy podróżują przez czarną dziurę w poszukiwaniu nowego domu dla ludzkości.", "Christopher Nolan", "Sci-Fi", "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg", 11.99m, 8.5999999999999996, "Interstellar", 2014 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
