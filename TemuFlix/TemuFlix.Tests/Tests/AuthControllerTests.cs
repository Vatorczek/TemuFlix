using Microsoft.AspNetCore.Mvc;
using TemuFlix.Controllers;
using TemuFlix.Models;
using TemuFlix.Services;
using TemuFlix.Tests.Helpers;
using Microsoft.Extensions.Configuration;

namespace TemuFlix.Tests.Tests
{
    public class AuthControllerTests
    {
        private JwtService CreateJwtService()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "TemuFlixSuperTajnyKluczDoJWT2025!XYZ",
                    ["Jwt:Issuer"] = "TemuFlix",
                    ["Jwt:Audience"] = "TemuFlixUsers",
                    ["Jwt:ExpiryHours"] = "24"
                })
                .Build();
            return new JwtService(config);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsOkWithToken()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Register_Test");
            var controller = new AuthController(db, CreateJwtService());

            // Act
            var result = await controller.Register(new RegisterRequest
            {
                Username = "testuser",
                Email = "test@test.com",
                Password = "haslo123"
            });

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<AuthResponse>>(ok.Value);
            Assert.True(response.Success);
            Assert.NotEmpty(response.Data!.Token);
            Assert.Equal("testuser", response.Data.Username);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Register_Duplicate_Test");
            var controller = new AuthController(db, CreateJwtService());

            await controller.Register(new RegisterRequest
            {
                Username = "user1",
                Email = "dup@test.com",
                Password = "haslo123"
            });

            // Act - próba rejestracji z tym samym emailem
            var result = await controller.Register(new RegisterRequest
            {
                Username = "user2",
                Email = "dup@test.com",
                Password = "haslo456"
            });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_ShortPassword_ReturnsBadRequest()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Register_ShortPass_Test");
            var controller = new AuthController(db, CreateJwtService());

            // Act
            var result = await controller.Register(new RegisterRequest
            {
                Username = "user",
                Email = "user@test.com",
                Password = "abc"
            });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Login_Test");
            var jwtService = CreateJwtService();
            var controller = new AuthController(db, jwtService);

            await controller.Register(new RegisterRequest
            {
                Username = "loginuser",
                Email = "login@test.com",
                Password = "haslo123"
            });

            // Act
            var result = controller.Login(new LoginRequest
            {
                Email = "login@test.com",
                Password = "haslo123"
            });

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<AuthResponse>>(ok.Value);
            Assert.True(response.Success);
            Assert.NotEmpty(response.Data!.Token);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsUnauthorized()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Login_WrongPass_Test");
            var controller = new AuthController(db, CreateJwtService());

            await controller.Register(new RegisterRequest
            {
                Username = "user",
                Email = "user@test.com",
                Password = "poprawnehaslo"
            });

            // Act
            var result = controller.Login(new LoginRequest
            {
                Email = "user@test.com",
                Password = "zlehaslo"
            });

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_NonExistingUser_ReturnsUnauthorized()
        {
            // Arrange
            var db = TestDbHelper.CreateInMemoryDb("Login_NoUser_Test");
            var controller = new AuthController(db, CreateJwtService());

            // Act
            var result = controller.Login(new LoginRequest
            {
                Email = "nieistnieje@test.com",
                Password = "haslo123"
            });

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}