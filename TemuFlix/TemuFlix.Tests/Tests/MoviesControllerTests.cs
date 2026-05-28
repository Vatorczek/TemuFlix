using Microsoft.AspNetCore.Mvc;
using TemuFlix.Controllers;
using TemuFlix.Models;
using TemuFlix.Tests.Helpers;

namespace TemuFlix.Tests.Tests
{
    public class MoviesControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOk_WithMovieList()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("GetAll_Test");
            var controller = new MoviesController(db);

            // Act
            var result = await controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<Movie>>>(ok.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsMovie()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("GetById_Test");
            db.Movies.Add(new Movie { Id = 100, Title = "Test Film", Year = 2020 });
            await db.SaveChangesAsync();
            var controller = new MoviesController(db);

            // Act
            var result = await controller.GetById(100);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Movie>>(ok.Value);
            Assert.Equal("Test Film", response.Data!.Title);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("GetById_NotFound_Test");
            var controller = new MoviesController(db);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ValidMovie_ReturnsCreated()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Create_Test");
            var controller = new MoviesController(db);
            var movie = new Movie
            {
                Title = "Nowy Film",
                Year = 2024,
                Director = "Jan Kowalski",
                Genre = "Action",
                Rating = 7.5,
                PriceUSD = 9.99m
            };

            // Act
            var result = await controller.Create(movie);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<Movie>>(created.Value);
            Assert.True(response.Success);
            Assert.Equal("Nowy Film", response.Data!.Title);
        }

        [Fact]
        public async Task Create_EmptyTitle_ReturnsBadRequest()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Create_BadRequest_Test");
            var controller = new MoviesController(db);
            var movie = new Movie { Title = "", Year = 2024 };

            // Act
            var result = await controller.Create(movie);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingMovie_ReturnsOk()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Delete_Test");
            db.Movies.Add(new Movie { Id = 200, Title = "Do usunięcia", Year = 2020 });
            await db.SaveChangesAsync();
            var controller = new MoviesController(db);

            // Act
            var result = await controller.Delete(200);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Null(await db.Movies.FindAsync(200));
        }

        [Fact]
        public async Task Delete_NonExistingMovie_ReturnsNotFound()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Delete_NotFound_Test");
            var controller = new MoviesController(db);

            // Act
            var result = await controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_ExistingMovie_ReturnsUpdated()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Update_Test");
            db.Movies.Add(new Movie { Id = 300, Title = "Stary tytuł", Year = 2010 });
            await db.SaveChangesAsync();
            var controller = new MoviesController(db);
            var updated = new Movie { Title = "Nowy tytuł", Year = 2024, Director = "Reżyser" };

            // Act
            var result = await controller.Update(300, updated);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Movie>>(ok.Value);
            Assert.Equal("Nowy tytuł", response.Data!.Title);
        }
    }
}