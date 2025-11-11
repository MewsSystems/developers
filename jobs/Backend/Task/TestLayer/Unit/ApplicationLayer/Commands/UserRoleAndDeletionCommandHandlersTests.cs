using ApplicationLayer.Commands.Users.ChangeUserRole;
using ApplicationLayer.Commands.Users.DeleteUser;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Enums;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for ChangeUserRole and DeleteUser command handlers.
/// </summary>
public class UserRoleAndDeletionCommandHandlersTests : TestBase
{
    #region ChangeUserRoleCommandHandler Tests

    [Fact]
    public async Task ChangeUserRole_WithValidUserAndRole_ShouldChangeRoleSuccessfully()
    {
        // Arrange
        var handler = new ChangeUserRoleCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ChangeUserRoleCommandHandler>().Object);

        var command = new ChangeUserRoleCommand(
            UserId: 1,
            NewRole: UserRole.Consumer);

        var user = User.Create("user@example.com", "hash", "John", "Doe", UserRole.Consumer);

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
        user.Role.Should().Be(UserRole.Consumer);

        MockUserRepository.Verify(x => x.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task ChangeUserRole_WithNonExistentUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new ChangeUserRoleCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ChangeUserRoleCommandHandler>().Object);

        var command = new ChangeUserRoleCommand(
            UserId: 999,
            NewRole: UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await handler.Handle(command, CancellationToken.None)
        );

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task ChangeUserRole_ChangingLastAdmin_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ChangeUserRoleCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ChangeUserRoleCommandHandler>().Object);

        var command = new ChangeUserRoleCommand(
            UserId: 1,
            NewRole: UserRole.Consumer);

        // This is the only admin
        var adminUser = User.Create("admin@example.com", "hash", "Admin", "User", UserRole.Admin);
        var regularUser = User.Create("user@example.com", "hash", "Regular", "User", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(adminUser);

        // Only one admin in the system
        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { adminUser, regularUser });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("last admin");

        // Verify role was not changed
        adminUser.Role.Should().Be(UserRole.Admin);
        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task ChangeUserRole_ChangingOneOfMultipleAdmins_ShouldSucceed()
    {
        // Arrange
        var handler = new ChangeUserRoleCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ChangeUserRoleCommandHandler>().Object);

        var command = new ChangeUserRoleCommand(
            UserId: 1,
            NewRole: UserRole.Consumer);

        var admin1 = User.Create("admin1@example.com", "hash", "Admin", "One", UserRole.Admin);
        var admin2 = User.Create("admin2@example.com", "hash", "Admin", "Two", UserRole.Admin);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(admin1);

        // Multiple admins in the system
        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { admin1, admin2 });

        MockUserRepository
            .Setup(x => x.UpdateAsync(admin1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        admin1.Role.Should().Be(UserRole.Consumer);
        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task ChangeUserRole_ChangingAdminToAdmin_ShouldSucceed()
    {
        // Arrange
        var handler = new ChangeUserRoleCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ChangeUserRoleCommandHandler>().Object);

        var command = new ChangeUserRoleCommand(
            UserId: 1,
            NewRole: UserRole.Admin);

        var adminUser = User.Create("admin@example.com", "hash", "Admin", "User", UserRole.Admin);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(adminUser);

        MockUserRepository
            .Setup(x => x.UpdateAsync(adminUser, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        adminUser.Role.Should().Be(UserRole.Admin);
        VerifySaveChangesCalled();
    }

    #endregion

    #region DeleteUserCommandHandler Tests

    [Fact]
    public async Task DeleteUser_WithValidNonAdminUser_ShouldDeleteSuccessfully()
    {
        // Arrange
        var handler = new DeleteUserCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteUserCommandHandler>().Object);

        var command = new DeleteUserCommand(UserId: 1);

        var user = User.Create("user@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        MockUserRepository
            .Setup(x => x.DeleteAsync(user, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockUserRepository.Verify(x => x.DeleteAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task DeleteUser_WithNonExistentUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new DeleteUserCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteUserCommandHandler>().Object);

        var command = new DeleteUserCommand(UserId: 999);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await handler.Handle(command, CancellationToken.None)
        );

        MockUserRepository.Verify(x => x.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task DeleteUser_DeletingLastAdmin_ShouldReturnFailure()
    {
        // Arrange
        var handler = new DeleteUserCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteUserCommandHandler>().Object);

        var command = new DeleteUserCommand(UserId: 1);

        // This is the only admin
        var adminUser = User.Create("admin@example.com", "hash", "Admin", "User", UserRole.Admin);
        var regularUser = User.Create("user@example.com", "hash", "Regular", "User", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(adminUser);

        // Only one admin in the system
        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { adminUser, regularUser });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("last admin");

        MockUserRepository.Verify(x => x.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task DeleteUser_DeletingOneOfMultipleAdmins_ShouldSucceed()
    {
        // Arrange
        var handler = new DeleteUserCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteUserCommandHandler>().Object);

        var command = new DeleteUserCommand(UserId: 1);

        var admin1 = User.Create("admin1@example.com", "hash", "Admin", "One", UserRole.Admin);
        var admin2 = User.Create("admin2@example.com", "hash", "Admin", "Two", UserRole.Admin);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(admin1);

        // Multiple admins in the system
        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { admin1, admin2 });

        MockUserRepository
            .Setup(x => x.DeleteAsync(admin1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockUserRepository.Verify(x => x.DeleteAsync(admin1, It.IsAny<CancellationToken>()), Times.Once);
        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task DeleteUser_WithManagerRole_ShouldDeleteSuccessfully()
    {
        // Arrange
        var handler = new DeleteUserCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteUserCommandHandler>().Object);

        var command = new DeleteUserCommand(UserId: 1);

        var manager = User.Create("manager@example.com", "hash", "Manager", "User", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(manager);

        MockUserRepository
            .Setup(x => x.DeleteAsync(manager, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockUserRepository.Verify(x => x.DeleteAsync(manager, It.IsAny<CancellationToken>()), Times.Once);
        VerifySaveChangesCalled();
    }

    #endregion
}
