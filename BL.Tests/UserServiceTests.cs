using System;
using System.Threading.Tasks;
using BL.Models.DTO;
using BL.Models.Requests;
using BL.Services.TokenService;
using BL.Services.UserService;
using DAL.Entities;
using DAL.Repositories.UserRepository.UserRepository;
using Lection2_Core_BL.Services.HashService;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BL.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IHashService> _hashServiceMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<ILogger<UserService>> _loggerMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            // Создание моков для зависимостей
            _userRepositoryMock = new Mock<IUserRepository>();
            _hashServiceMock = new Mock<IHashService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _loggerMock = new Mock<ILogger<UserService>>();

            // Инициализация сервиса с моками
            _userService = new UserService(
                _userRepositoryMock.Object,
                _hashServiceMock.Object,
                _tokenServiceMock.Object,
                _loggerMock.Object
            );
        }

        #region RegisterAsync Tests

        [Test]
        public void RegisterAsync_WhenNullRegistrationRequest_ShouldThrowsArgumentNullException()
        {
            // Arrange
            RegistrationRequest registrationRequest = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.RegisterAsync(registrationRequest));
            Assert.That(ex.ParamName, Is.EqualTo("registrationRequest"));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("registrationRequest is null")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void RegisterAsync_WhenEmptyEmail_ShouldThrowsArgumentException()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest
            {
                Email = "",
                Password = "Password123!",
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterAsync(registrationRequest));
            Assert.That(ex.Message, Does.Contain("Email cannot be empty."));

            // Проверка логирования ошибки
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Email is empty")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void RegisterAsync_WhenEmptyPassword_ShouldThrowsArgumentException()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest
            {
                Email = "user@example.com",
                Password = "",
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterAsync(registrationRequest));
            Assert.That(ex.Message, Does.Contain("Password cannot be empty."));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Password is empty")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void RegisterAsync_WhenUserAlreadyExists_ShouldThrowsInvalidOperationException()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest
            {
                Email = "existinguser@example.com",
                Password = "Password123!",
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "existinguser@example.com",
                PasswordHash = "hashedpassword",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registrationRequest.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.RegisterAsync(registrationRequest));
            Assert.That(ex.Message, Is.EqualTo($"User with email {registrationRequest.Email} already exists."));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"User with email {registrationRequest.Email} already exists.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public async Task RegisterAsync_WhenSuccessfulRegistration_ShouldCallsCreateAsync()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest
            {
                Email = "newuser@example.com",
                Password = "Password123!",
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registrationRequest.Email))
                .ReturnsAsync((User?)null);

            _hashServiceMock.Setup(hash => hash.GetHash(registrationRequest.Password))
                .Returns("hashedpassword");

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            // Act
            await _userService.RegisterAsync(registrationRequest);

            // Assert
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<User>(
                u => u.Email == registrationRequest.Email &&
                     u.PasswordHash == "hashedpassword"
            )), Times.Once);

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"User registered successfully with email: {registrationRequest.Email}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void RegisterAsync_WhenRepositoryThrowsException_ShouldThrowsException()
        {
            // Arrange
            var registrationRequest = new RegistrationRequest
            {
                Email = "newuser@example.com",
                Password = "Password123!",
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registrationRequest.Email))
                .ReturnsAsync((User?)null);

            _hashServiceMock.Setup(hash => hash.GetHash(registrationRequest.Password))
                .Returns("hashedpassword");

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _userService.RegisterAsync(registrationRequest));
            Assert.That(ex.Message, Is.EqualTo("An error occurred while registering the user."));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"An error occurred while registering the user with email: {registrationRequest.Email}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        #endregion

        #region LoginAsync Tests

        [Test]
        public void LoginAsync_WhenNullLoginDto_ShouldThrowsArgumentNullException()
        {
            // Arrange
            LoginDto loginDto = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.LoginAsync(loginDto));
            Assert.That(ex.ParamName, Is.EqualTo("loginDto"));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("loginDto is null")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void LoginAsync_WhenEmptyEmail_ShouldThrowsArgumentException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "",
                Password = "Password123!"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.LoginAsync(loginDto));
            Assert.That(ex.Message, Does.Contain("Email cannot be empty."));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Email is empty")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void LoginAsync_WhenEmptyPassword_ShouldThrowsArgumentException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "user@example.com",
                Password = ""
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.LoginAsync(loginDto));
            Assert.That(ex.Message, Does.Contain("Password cannot be empty."));

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Password is empty")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public async Task LoginAsync_WhenNonExistentUser_ShouldReturnsEmptyString()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "nonexistent@example.com",
                Password = "Password123!"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.That(result, Is.Empty);

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Login failed: No user found with email {loginDto.Email}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public async Task LoginAsync_WhenIncorrectPassword_ShouldReturnsEmptyString()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "user@example.com",
                Password = "WrongPassword!"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                PasswordHash = "hashedpassword"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            _hashServiceMock.Setup(hash => hash.VerifySameHash(loginDto.Password, user.PasswordHash))
                .Returns(false);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.That(result, Is.Empty);

            // Shoul call VerifySameHash
            _hashServiceMock.Verify(hash => hash.VerifySameHash(loginDto.Password, user.PasswordHash), Times.Once);

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Login failed: Incorrect password for user with email {loginDto.Email}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public async Task LoginAsync_WhenSuccessfulLogin_ShouldReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@example.com",
                PasswordHash = "hashedpassword"
            };

            var expectedToken = "generated.jwt.token";

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            _hashServiceMock.Setup(hash => hash.VerifySameHash(loginDto.Password, user.PasswordHash))
                .Returns(true);

            _tokenServiceMock.Setup(token => token.GenerateToken(user.Id))
                .Returns(expectedToken);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.That(expectedToken, Is.EqualTo(result));

            // Should call VerifySameHash and GenerateToken
            _hashServiceMock.Verify(hash => hash.VerifySameHash(loginDto.Password, user.PasswordHash), Times.Once);
            _tokenServiceMock.Verify(token => token.GenerateToken(user.Id), Times.Once);

            // Logging check
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Login successful for user with email: {loginDto.Email}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        [Test]
        public void LoginAsync_WhenRepositoryThrowsException_ShouldThrowsException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                .ThrowsAsync(new Exception("An error occurred while generating the token."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _userService.LoginAsync(loginDto));
            Assert.That(ex.Message, Is.EqualTo("An error occurred while generating the token."));

        }

        #endregion
    }
}
