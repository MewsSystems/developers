using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Commands.Users.UpdateUserInfo;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.Users;

public class UpdateUserInfoCommandTests : IntegrationTestBase
{
    private async Task<int> CreateTestUser(string? email = null)
    {
        var command = new CreateUserCommand(
            Email: email ?? $"test_{Guid.NewGuid():N}@example.com",
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer
        );

        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task UpdateUserInfo_WithValidData_ShouldUpdateUser()
    {
        // Arrange
        var userId = await CreateTestUser();

        var command = new UpdateUserInfoCommand(
            UserId: userId,
            FirstName: "Jane",
            LastName: "Smith",
            Email: "jane@example.com"
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue($"but got error: {result.Error}");

        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        user.Should().NotBeNull();
        user!.FirstName.Should().Be("Jane");
        user.LastName.Should().Be("Smith");
        user.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public async Task UpdateUserInfo_WithNonExistentUser_ShouldFail()
    {
        // Arrange
        var command = new UpdateUserInfoCommand(
            UserId: 999,
            FirstName: "Jane",
            LastName: "Smith",
            Email: "jane@example.com"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.NotFoundException>(
            async () => await Mediator.Send(command)
        );
    }

    [Fact]
    public async Task UpdateUserInfo_WithExistingEmail_ShouldFail()
    {
        // Arrange
        var user1Email = $"user1_{Guid.NewGuid():N}@example.com";
        var user2Email = $"user2_{Guid.NewGuid():N}@example.com";

        await CreateTestUser(user1Email);
        var user2Id = await CreateTestUser(user2Email);

        var command = new UpdateUserInfoCommand(
            UserId: user2Id,
            FirstName: "Jane",
            LastName: "Smith",
            Email: user1Email // Email already taken by user1
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already in use");
    }

    [Fact]
    public async Task UpdateUserInfo_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var userId = await CreateTestUser();

        var command = new UpdateUserInfoCommand(
            UserId: userId,
            FirstName: "Jane",
            LastName: "Smith",
            Email: "invalid-email"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("Email");
    }
}
