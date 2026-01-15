using System.Reflection.Metadata;
using FluentAssertions;
using FluentEmail.Core;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RiffBackend.Application.Common;
using RiffBackend.Application.Services;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;
using RiffBackend.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Tests
{
    public class UserServiceTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _filestorage;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProivder;
        private readonly IFileProcessor _fileProcessor;
        private readonly IEmailVerificationLinkFactory _linkFactory;
        private readonly IFluentEmail _fluentEmail;
        private readonly UserService _userService;
        private readonly User user = User.Create(Guid.NewGuid(), "test", "test@email.com", "1234", User.DEFAULT_AVATAR_PATH, true);

        public UserServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _filestorage = Substitute.For<IFileStorageService>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _jwtProivder = Substitute.For<IJwtProvider>();
            _fileProcessor = Substitute.For<IFileProcessor>();
            _linkFactory = Substitute.For<IEmailVerificationLinkFactory>();
            _fluentEmail = Substitute.For<IFluentEmail>();

            _userService = new UserService(_userRepository, _filestorage, 
                            _passwordHasher, _jwtProivder, _fileProcessor, _linkFactory, _fluentEmail);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnError_WhenUserNotFound()
        {
            // Arrange

            _userRepository.GetUserByIdAsync(user.Id).ReturnsNull();

            // Act
            var result = await _userService.GetByIdAsync(user.Id);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.NotFound(user.Id));
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnError_WhenIdIsEmpty()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            var result = await _userService.GetByIdAsync(id);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.MissingId());
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnError_WhenFilePathIsEmpty()
        {
            // Arrange
            _userRepository.GetUserByIdAsync(user.Id).Returns(user);
            _filestorage.GetURLAsync(user.AvatarPath).Returns(Errors.FileErrors.MissingFilePath());

            // Act
            var result = await _userService.GetByIdAsync(user.Id);

            // Assert
            result.Error.Should().Be(Errors.FileErrors.MissingFilePath());
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnSucces()
        {
            // Arrange
            _userRepository.GetUserByIdAsync(user.Id).Returns(user);
            _filestorage.GetURLAsync(user.AvatarPath).Returns("str");

            // Act
            var result = await _userService.GetByIdAsync(user.Id);

            // Assert
            result.Value.Should().Be(user);
        }

        [Fact]
        public async Task GetUserFromJwtAsync_Should_ReturnError_WhenJwtNull()
        {
            // Arrange

            // Act
            var result = await _userService.GetUserFromJwtAsync("");

            // Assert
            result.Error.Should().Be(Errors.General.ValueIsRequired("JWT"));
        }

        [Fact]
        public async Task GetUserFromJwtAsync_Should_ReturnError_WhenUserNotFound()
        {
            // Arrange
            _userRepository.GetUserByIdAsync(user.Id).ReturnsNull();
            _jwtProivder.GetGuidFromJwt("jwt").Returns(Guid.Empty);
            // Act
            var result = await _userService.GetUserFromJwtAsync("jwt");

            // Assert
            result.Error.Should().Be(Errors.UserErrors.NotFound(Guid.Empty));
        }

        [Fact]
        public async Task GetUserFromJwtAsync_Should_ReturnSuccess()
        {
            // Arrange
            _userRepository.GetUserByIdAsync(user.Id).Returns(user);
            _jwtProivder.GetGuidFromJwt("jwt").Returns(user.Id);
            _filestorage.GetURLAsync(user.AvatarPath).Returns("str");

            // Act
            var result = await _userService.GetUserFromJwtAsync("jwt");

            // Assert
            result.Value.Should().Be(user);
        }

        [Fact]
        public async Task RegisterAsync_Should_ReturnError_EmailDuplicate()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).Returns(user);
            // Act
            var result = await _userService.RegisterAsync(user.Id, user.Name, user.Email, user.PasswordHash, default, default);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.EmailDuplicate(user.Email));
        }

        [Fact]
        public async Task RegisterAsync_Should_ReturnSuccess()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).ReturnsNull();
            _passwordHasher.Hash(user.PasswordHash).Returns(user.PasswordHash);
            _fileProcessor.UploadNewOrKeepOldAsync(null, User.DEFAULT_AVATAR_PATH, default, _filestorage.UploadImageFileAsync)
                                .Returns(User.DEFAULT_AVATAR_PATH);
            // Act
            var result = await _userService.RegisterAsync(Guid.NewGuid(), user.Name, user.Email, user.PasswordHash, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnError_WhenIdIsEmpty()
        {
            // Arrange

            // Act
            var result = await _userService.UpdateAsync(Guid.Empty, user.Name, user.Email, user.PasswordHash, default);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.MissingId());
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnError_When_Email_Duplicate()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).Returns(user);

            // Act
            var result = await _userService.UpdateAsync(Guid.NewGuid(), user.Name, user.Email, user.PasswordHash, default);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.EmailDuplicate(user.Email));
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnError_When_UserNotFound()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).ReturnsNull();
            _userRepository.GetUserByIdAsync(user.Id).ReturnsNull();

            // Act
            var result = await _userService.UpdateAsync(user.Id, user.Name, user.Email, user.PasswordHash, default);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.NotFound(user.Id));
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnSuccess()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).ReturnsNull();
            _userRepository.GetUserByIdAsync(user.Id).Returns(user);
            _fileProcessor.UploadNewOrKeepOldAsync(null, user.AvatarPath, default, _filestorage.UploadImageFileAsync)
                                .Returns(User.DEFAULT_AVATAR_PATH);

            // Act
            var result = await _userService.UpdateAsync(user.Id, user.Name, user.Email, user.PasswordHash, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnSuccess()
        {
            // Arrange
            _userRepository.GetUserByIdAsync(user.Id).Returns(user);

            // Act
            var result = await _userService.DeleteAsync(user.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnError_WhenIdIsEmpty()
        {
            // Arrange

            // Act
            var result = await _userService.DeleteAsync(Guid.Empty);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.MissingId());
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnError_WhenUserNotFound()
        {
            // Arrange
            _userRepository.GetUserByIdAsync(user.Id).ReturnsNull();

            // Act
            var result = await _userService.DeleteAsync(user.Id);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.NotFound(user.Id));
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnSuccess()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).Returns(user);
            _passwordHasher.Verify(user.PasswordHash, user.PasswordHash).Returns(true);

            // Act
            var result = await _userService.LoginAsync(user.Email, user.PasswordHash);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnError_WhenUserNotFound()
        {
            // Arrange
            _userRepository.GetByEmailAsync(user.Email).ReturnsNull();

            // Act
            var result = await _userService.LoginAsync(user.Email, user.PasswordHash);

            // Assert
            result.Error.Should().Be(Errors.UserErrors.NotFound(null,user.Email));
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnError_WhenEmailIsEmpty()
        {
            // Arrange

            // Act
            var result = await _userService.LoginAsync("", user.PasswordHash);

            // Assert
            result.Error.Should().Be(Errors.General.ValueIsRequired("почта"));
        }

        [Fact]
        public async Task LoginAsync_Should_ReturnError_WhenPasswordIsEmpty()
        {
            // Arrange

            // Act
            var result = await _userService.LoginAsync(user.Email, "");

            // Assert
            result.Error.Should().Be(Errors.General.ValueIsRequired("пароль"));
        }
    }
}
