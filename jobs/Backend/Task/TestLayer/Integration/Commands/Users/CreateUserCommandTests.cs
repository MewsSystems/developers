using ApplicationLayer.Commands.Users.CreateUser;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.Users;

public class CreateUserCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateUser_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var uniqueEmail = $"test_{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            Email: uniqueEmail,
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        // Verify user was created in database
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Email == uniqueEmail);

        user.Should().NotBeNull();
        user!.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        user.Role.Should().Be("Consumer");
        user.PasswordHash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ShouldFail()
    {
        // Arrange - Create first user
        var uniqueEmail = $"test_{Guid.NewGuid():N}@example.com";
        var firstCommand = new CreateUserCommand(
            Email: uniqueEmail,
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer
        );
        await Mediator.Send(firstCommand);

        // Act - Try to create second user with same email
        var secondCommand = new CreateUserCommand(
            Email: uniqueEmail,
            Password: "Test456!@#",
            FirstName: "Jane",
            LastName: "Smith",
            Role: UserRole.Admin
        );
        var result = await Mediator.Send(secondCommand);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");
    }

    [Fact]
    public async Task CreateUser_WithWeakPassword_ShouldFail()
    {
        // Arrange
        var uniqueEmail = $"test_{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            Email: uniqueEmail,
            Password: "weak",
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("Password");
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "invalid-email",
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public async Task CreateUser_AsAdmin_ShouldCreateAdminUser()
    {
        // Arrange
        var uniqueEmail = $"admin_{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            Email: uniqueEmail,
            Password: "Admin123!@#",
            FirstName: "Admin",
            LastName: "User",
            Role: UserRole.Admin
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Email == uniqueEmail);

        user.Should().NotBeNull();
        user!.Role.Should().Be("Admin");
    }
}
