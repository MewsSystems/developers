using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Commands.Users.DeleteUser;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.Users;

public class DeleteUserCommandTests : IntegrationTestBase
{
    private async Task<int> CreateTestUser(UserRole role = UserRole.Consumer)
    {
        var command = new CreateUserCommand(
            Email: $"user_{Guid.NewGuid():N}@example.com",
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: role
        );

        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task DeleteUser_WithValidUser_ShouldDeleteUser()
    {
        // Arrange
        var userId = await CreateTestUser();

        var command = new DeleteUserCommand(userId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify user was deleted from database
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        user.Should().BeNull();
    }

    [Fact]
    public async Task DeleteUser_WithNonExistentUser_ShouldFail()
    {
        // Arrange
        var command = new DeleteUserCommand(999);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.NotFoundException>(
            async () => await Mediator.Send(command)
        );
    }

    [Fact]
    public async Task DeleteUser_WithLastAdmin_ShouldFail()
    {
        // Arrange - Create a single admin user
        var adminId = await CreateTestUser(UserRole.Admin);

        var command = new DeleteUserCommand(adminId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("last admin");

        // Verify user still exists
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == adminId);

        user.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteUser_WithOneOfMultipleAdmins_ShouldSucceed()
    {
        // Arrange - Create two admin users
        var admin1Id = await CreateTestUser(UserRole.Admin);
        var admin2Id = await CreateTestUser(UserRole.Admin);

        var command = new DeleteUserCommand(admin1Id);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify first admin was deleted
        var user1 = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == admin1Id);
        user1.Should().BeNull();

        // Verify second admin still exists
        var user2 = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == admin2Id);
        user2.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteUser_WithConsumerUser_ShouldAlwaysSucceed()
    {
        // Arrange
        var consumerId = await CreateTestUser(UserRole.Consumer);

        var command = new DeleteUserCommand(consumerId);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify user was deleted
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == consumerId);

        user.Should().BeNull();
    }
}
