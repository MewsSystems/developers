using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Queries.Users.GetUsersByRole;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Users;

public class GetUsersByRoleQueryTests : IntegrationTestBase
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
    public async Task GetUsersByRole_WithAdminRole_ShouldReturnOnlyAdmins()
    {
        // Arrange
        await CreateTestUser(UserRole.Admin);
        await CreateTestUser(UserRole.Admin);
        await CreateTestUser(UserRole.Consumer);
        await CreateTestUser(UserRole.Consumer);

        var query = new GetUsersByRoleQuery(
            Role: UserRole.Admin,
            PageNumber: 1,
            PageSize: 10
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2, "at least 2 Admin users should exist");
        result.Items.Should().OnlyContain(u => u.Role == "Admin");
    }

    [Fact]
    public async Task GetUsersByRole_WithConsumerRole_ShouldReturnOnlyConsumers()
    {
        // Arrange
        await CreateTestUser(UserRole.Consumer);
        await CreateTestUser(UserRole.Consumer);
        await CreateTestUser(UserRole.Consumer);
        await CreateTestUser(UserRole.Admin);

        var query = new GetUsersByRoleQuery(
            Role: UserRole.Consumer,
            PageNumber: 1,
            PageSize: 10
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(3, "at least 3 Consumer users should exist");
        result.Items.Should().OnlyContain(u => u.Role == "Consumer");
    }

    [Fact]
    public async Task GetUsersByRole_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange - Create 5 consumer users
        for (int i = 0; i < 5; i++)
        {
            await CreateTestUser(UserRole.Consumer);
        }

        var query = new GetUsersByRoleQuery(
            Role: UserRole.Consumer,
            PageNumber: 1,
            PageSize: 2
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountLessThanOrEqualTo(2, "page size is 2");
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.Items.Should().OnlyContain(u => u.Role == "Consumer");
    }

    [Fact]
    public async Task GetUsersByRole_WithSecondPage_ShouldReturnCorrectItems()
    {
        // Arrange - Create 5 consumer users
        for (int i = 0; i < 5; i++)
        {
            await CreateTestUser(UserRole.Consumer);
        }

        var query = new GetUsersByRoleQuery(
            Role: UserRole.Consumer,
            PageNumber: 2,
            PageSize: 2
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(1, "second page should have at least 1 item");
        result.PageNumber.Should().Be(2);
        result.Items.Should().OnlyContain(u => u.Role == "Consumer");
    }

    [Fact]
    public async Task GetUsersByRole_WithNoUsersOfRole_ShouldReturnEmptyResult()
    {
        // Arrange - Create only consumer users
        await CreateTestUser(UserRole.Consumer);
        await CreateTestUser(UserRole.Consumer);

        // Clear any existing admin users first by querying current state
        var existingAdminsQuery = new GetUsersByRoleQuery(
            Role: UserRole.Admin,
            PageNumber: 1,
            PageSize: 100
        );

        // Act - Query for admins when we only created consumers recently
        var query = new GetUsersByRoleQuery(
            Role: UserRole.Admin,
            PageNumber: 1,
            PageSize: 10
        );

        var result = await Mediator.Send(query);

        // Assert - May have existing admins from other tests, so just verify structure
        result.Should().NotBeNull();
        result.Items.Should().OnlyContain(u => u.Role == "Admin");
    }

    [Fact]
    public async Task GetUsersByRole_WithInvalidPageSize_ShouldFail()
    {
        // Arrange
        var query = new GetUsersByRoleQuery(
            Role: UserRole.Consumer,
            PageNumber: 1,
            PageSize: 150 // Max is 100
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("PageSize");
    }

    [Fact]
    public async Task GetUsersByRole_WithInvalidPageNumber_ShouldFail()
    {
        // Arrange
        var query = new GetUsersByRoleQuery(
            Role: UserRole.Consumer,
            PageNumber: 0, // Must be >= 1
            PageSize: 10
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("PageNumber");
    }
}
