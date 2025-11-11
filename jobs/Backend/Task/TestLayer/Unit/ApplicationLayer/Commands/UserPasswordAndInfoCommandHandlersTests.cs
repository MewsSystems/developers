using ApplicationLayer.Commands.Users.ChangeUserPassword;
using ApplicationLayer.Commands.Users.UpdateUserInfo;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Enums;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for ChangeUserPassword and UpdateUserInfo command handlers.
/// </summary>
public class UserPasswordAndInfoCommandHandlersTests : TestBase
{
    #region ChangeUserPasswordCommandHandler Tests

    [Fact]
    public async Task ChangeUserPassword_WithCorrectCurrentPassword_ShouldChangePasswordSuccessfully()
    {
        // Arrange
        var handler = new ChangeUserPasswordCommandHandler(
            MockUnitOfWork.Object,
            MockPasswordHasher.Object,
            CreateMockLogger<ChangeUserPasswordCommandHandler>().Object);

        var command = new ChangeUserPasswordCommand(
            UserId: 1,
            CurrentPassword: "OldPassword123",
            NewPassword: "NewSecurePassword456");

        var user = User.Create("user@example.com", "old_hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Mock: Verify current password succeeds
        MockPasswordHasher
            .Setup(x => x.VerifyPassword("OldPassword123", "old_hash"))
            .Returns(true);

        // Mock: Hash new password
        MockPasswordHasher
            .Setup(x => x.HashPassword("NewSecurePassword456"))
            .Returns("new_secure_hash");

        MockUserRepository
            .Setup(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockPasswordHasher.Verify(x => x.VerifyPassword("OldPassword123", "old_hash"), Times.Once);
        MockPasswordHasher.Verify(x => x.HashPassword("NewSecurePassword456"), Times.Once);
        MockUserRepository.Verify(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task ChangeUserPassword_WithIncorrectCurrentPassword_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ChangeUserPasswordCommandHandler(
            MockUnitOfWork.Object,
            MockPasswordHasher.Object,
            CreateMockLogger<ChangeUserPasswordCommandHandler>().Object);

        var command = new ChangeUserPasswordCommand(
            UserId: 1,
            CurrentPassword: "WrongPassword",
            NewPassword: "NewPassword123");

        var user = User.Create("user@example.com", "correct_hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Mock: Verify current password fails
        MockPasswordHasher
            .Setup(x => x.VerifyPassword("WrongPassword", "correct_hash"))
            .Returns(false);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Current password is incorrect");

        // Verify new password was never hashed or saved
        MockPasswordHasher.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
        MockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task ChangeUserPassword_WithNonExistentUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new ChangeUserPasswordCommandHandler(
            MockUnitOfWork.Object,
            MockPasswordHasher.Object,
            CreateMockLogger<ChangeUserPasswordCommandHandler>().Object);

        var command = new ChangeUserPasswordCommand(
            UserId: 999,
            CurrentPassword: "Password123",
            NewPassword: "NewPassword456");

        MockUserRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task ChangeUserPassword_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ChangeUserPasswordCommandHandler(
            MockUnitOfWork.Object,
            MockPasswordHasher.Object,
            CreateMockLogger<ChangeUserPasswordCommandHandler>().Object);

        var command = new ChangeUserPasswordCommand(
            UserId: 1,
            CurrentPassword: "OldPassword",
            NewPassword: "NewPassword");

        var user = User.Create("user@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        MockPasswordHasher
            .Setup(x => x.VerifyPassword("OldPassword", "hash"))
            .Returns(true);

        MockPasswordHasher
            .Setup(x => x.HashPassword("NewPassword"))
            .Returns("new_hash");

        MockUserRepository
            .Setup(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to change password");
    }

    [Fact]
    public async Task ChangeUserPassword_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new ChangeUserPasswordCommandHandler(
            MockUnitOfWork.Object,
            MockPasswordHasher.Object,
            CreateMockLogger<ChangeUserPasswordCommandHandler>().Object);

        var command = new ChangeUserPasswordCommand(1, "Old", "New");
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var user = User.Create("user@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(user);
        MockPasswordHasher.Setup(x => x.VerifyPassword("Old", "hash")).Returns(true);
        MockPasswordHasher.Setup(x => x.HashPassword("New")).Returns("new_hash");
        MockUserRepository.Setup(x => x.UpdateAsync(user, token)).Returns(Task.CompletedTask);
        MockUnitOfWork.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(1);

        // Act
        await handler.Handle(command, token);

        // Assert
        MockUserRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockUserRepository.Verify(x => x.UpdateAsync(user, token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    #endregion

    #region UpdateUserInfoCommandHandler Tests

    [Fact]
    public async Task UpdateUserInfo_WithValidNewInfo_ShouldUpdateSuccessfully()
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(
            UserId: 1,
            FirstName: "Jane",
            LastName: "Smith",
            Email: "jane.smith@example.com");

        var user = User.Create("john.doe@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Email is changing, check it doesn't exist
        MockUserRepository
            .Setup(x => x.GetByEmailAsync("jane.smith@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockUserRepository
            .Setup(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockUserRepository.Verify(
            x => x.GetByEmailAsync("jane.smith@example.com", It.IsAny<CancellationToken>()),
            Times.Once);

        MockUserRepository.Verify(
            x => x.UpdateAsync(user, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task UpdateUserInfo_WithSameEmail_ShouldNotCheckForDuplicate()
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(
            UserId: 1,
            FirstName: "Jane",
            LastName: "Smith",
            Email: "john.doe@example.com"); // Same email

        var user = User.Create("john.doe@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        MockUserRepository
            .Setup(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify email check was NOT called (email didn't change)
        MockUserRepository.Verify(
            x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task UpdateUserInfo_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(
            UserId: 1,
            FirstName: "John",
            LastName: "Doe",
            Email: "existing@example.com");

        var user = User.Create("john@example.com", "hash", "John", "Doe", UserRole.Consumer);
        typeof(User).GetProperty("Id")!.SetValue(user, 1);

        var existingUser = User.Create("existing@example.com", "hash2", "Other", "User", UserRole.Consumer);
        typeof(User).GetProperty("Id")!.SetValue(existingUser, 2);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already in use");

        MockUserRepository.Verify(
            x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task UpdateUserInfo_WithNonExistentUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(999, "First", "Last", "email@example.com");

        MockUserRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        VerifySaveChangesNotCalled();
    }

    [Theory]
    [InlineData("", "Last", "email@example.com")]  // Empty first name
    [InlineData("First", "", "email@example.com")] // Empty last name
    [InlineData("First", "Last", "")]              // Empty email
    [InlineData("First", "Last", "invalidemail")]  // Invalid email format
    public async Task UpdateUserInfo_WithInvalidData_ShouldReturnFailure(
        string firstName,
        string lastName,
        string email)
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(1, firstName, lastName, email);
        var user = User.Create("user@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        if (!string.IsNullOrEmpty(email) && email != "invalidemail")
        {
            MockUserRepository
                .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
        }

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeEmpty();
        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task UpdateUserInfo_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(1, "First", "Last", "email@example.com");
        var user = User.Create("old@example.com", "hash", "Old", "Name", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("email@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockUserRepository
            .Setup(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to update user");
    }

    [Fact]
    public async Task UpdateUserInfo_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new UpdateUserInfoCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateUserInfoCommandHandler>().Object);

        var command = new UpdateUserInfoCommand(1, "First", "Last", "new@example.com");
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var user = User.Create("old@example.com", "hash", "Old", "Name", UserRole.Consumer);

        MockUserRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(user);
        MockUserRepository.Setup(x => x.GetByEmailAsync("new@example.com", token)).ReturnsAsync((User?)null);
        MockUserRepository.Setup(x => x.UpdateAsync(user, token)).Returns(Task.CompletedTask);
        MockUnitOfWork.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(1);

        // Act
        await handler.Handle(command, token);

        // Assert
        MockUserRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockUserRepository.Verify(x => x.GetByEmailAsync("new@example.com", token), Times.Once);
        MockUserRepository.Verify(x => x.UpdateAsync(user, token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    #endregion
}
