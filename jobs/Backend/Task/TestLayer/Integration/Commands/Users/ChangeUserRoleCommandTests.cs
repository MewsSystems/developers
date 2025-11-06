using ApplicationLayer.Commands.Users.ChangeUserRole;
using ApplicationLayer.Commands.Users.CreateUser;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Integration.Commands.Users;

public class ChangeUserRoleCommandTests : IntegrationTestBase
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
    public async Task ChangeUserRole_FromConsumerToAdmin_ShouldSucceed()
    {
        // Arrange
        var userId = await CreateTestUser(UserRole.Consumer);

        var command = new ChangeUserRoleCommand(
            UserId: userId,
            NewRole: UserRole.Admin
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify role was changed in database
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        user.Should().NotBeNull();
        user!.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task ChangeUserRole_FromAdminToConsumer_ShouldSucceed()
    {
        // Arrange - Create two admin users so we can change one
        var admin1Id = await CreateTestUser(UserRole.Admin);
        await CreateTestUser(UserRole.Admin); // Second admin

        var command = new ChangeUserRoleCommand(
            UserId: admin1Id,
            NewRole: UserRole.Consumer
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify role was changed
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == admin1Id);

        user.Should().NotBeNull();
        user!.Role.Should().Be("Consumer");
    }

    [Fact]
    public async Task ChangeUserRole_OfLastAdmin_ShouldFail()
    {
        // Arrange - Create a single admin user
        var adminId = await CreateTestUser(UserRole.Admin);

        var command = new ChangeUserRoleCommand(
            UserId: adminId,
            NewRole: UserRole.Consumer
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("last admin");

        // Verify role was NOT changed
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == adminId);

        user.Should().NotBeNull();
        user!.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task ChangeUserRole_WithNonExistentUser_ShouldFail()
    {
        // Arrange
        var command = new ChangeUserRoleCommand(
            UserId: 999,
            NewRole: UserRole.Admin
        );

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.NotFoundException>(
            async () => await Mediator.Send(command)
        );
    }

    [Fact]
    public async Task ChangeUserRole_ToSameRole_ShouldSucceed()
    {
        // Arrange
        var userId = await CreateTestUser(UserRole.Consumer);

        var command = new ChangeUserRoleCommand(
            UserId: userId,
            NewRole: UserRole.Consumer
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert - Should succeed even though it's the same role
        result.IsSuccess.Should().BeTrue();

        // Verify role is still Consumer
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        user.Should().NotBeNull();
        user!.Role.Should().Be("Consumer");
    }

    [Fact]
    public async Task ChangeUserRole_OfAdminWhenMultipleExist_ShouldSucceed()
    {
        // Arrange - Create three admin users
        var admin1Id = await CreateTestUser(UserRole.Admin);
        await CreateTestUser(UserRole.Admin);
        await CreateTestUser(UserRole.Admin);

        var command = new ChangeUserRoleCommand(
            UserId: admin1Id,
            NewRole: UserRole.Consumer
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify role was changed
        var user = await DbContext.Set<DataLayer.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id == admin1Id);

        user.Should().NotBeNull();
        user!.Role.Should().Be("Consumer");
    }
}
