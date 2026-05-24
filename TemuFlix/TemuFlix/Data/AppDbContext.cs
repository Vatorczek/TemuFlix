using Microsoft.EntityFrameworkCore;
using TemuFlix.Models;

namespace TemuFlix.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    Year = 2010,
                    Director = "Christopher Nolan",
                    Description = "Złodziej kradnący sekrety poprzez technologię wspólnych snów.",
                    Genre = "Sci-Fi",
                    Rating = 8.8,
                    PriceUSD = 9.99m,
                    PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_SX300.jpg"
                },
                new Movie
                {
                    Id = 2,
                    Title = "The Matrix",
                    Year = 1999,
                    Director = "The Wachowskis",
                    Description = "Haker odkrywa, że rzeczywistość jest symulacją.",
                    Genre = "Action",
                    Rating = 8.7,
                    PriceUSD = 7.99m,
                    PosterUrl = "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVlLTM5YTUtZjk4OGU3ODUxNzgxXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_SX300.jpg"
                },
                new Movie
                {
                    Id = 3,
                    Title = "Interstellar",
                    Year = 2014,
                    Director = "Christopher Nolan",
                    Description = "Eksploratorzy podróżują przez czarną dziurę w poszukiwaniu nowego domu dla ludzkości.",
                    Genre = "Sci-Fi",
                    Rating = 8.6,
                    PriceUSD = 11.99m,
                    PosterUrl = "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg"
                }
            );
        }
    }
}