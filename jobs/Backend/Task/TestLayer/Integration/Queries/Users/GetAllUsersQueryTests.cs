using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Queries.Users.GetAllUsers;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Users;

public class GetAllUsersQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestUser(string? email = null, UserRole role = UserRole.Consumer, string? firstName = null)
    {
        var actualEmail = email ?? $"user_{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            Email: actualEmail,
            Password: "Test123!@#",
            FirstName: firstName ?? actualEmail.Split('@')[0],
            LastName: "Test",
            Role: role
        );

        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task GetAllUsers_WithMultipleUsers_ShouldReturnPagedResults()
    {
        // Arrange - Create 5 test users with unique emails
        await CreateTestUser();
        await CreateTestUser();
        await CreateTestUser(role: UserRole.Admin);
        await CreateTestUser();
        await CreateTestUser();

        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 3);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(3, "at least 3 users should exist");
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(3);
    }

    [Fact]
    public async Task GetAllUsers_WithRoleFilter_ShouldReturnFilteredResults()
    {
        // Arrange - Create unique users for this test
        await CreateTestUser(role: UserRole.Consumer);
        await CreateTestUser(role: UserRole.Consumer);
        await CreateTestUser(role: UserRole.Admin);
        await CreateTestUser(role: UserRole.Admin);

        var query = new GetAllUsersQuery(
            PageNumber: 1,
            PageSize: 10,
            RoleFilter: UserRole.Admin
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2, "at least 2 Admin users should exist");
        result.Items.Should().OnlyContain(u => u.Role == "Admin");
    }

    [Fact]
    public async Task GetAllUsers_WithSearchTerm_ShouldReturnMatchingUsers()
    {
        // Arrange - Create users with unique emails but searchable term "doe"
        var guid1 = Guid.NewGuid().ToString("N");
        var guid2 = Guid.NewGuid().ToString("N");
        var guid3 = Guid.NewGuid().ToString("N");

        await CreateTestUser($"john.doe.{guid1}@example.com", firstName: "John");
        await CreateTestUser($"jane.doe.{guid2}@example.com", firstName: "Jane");
        await CreateTestUser($"alice.{guid3}@example.com", firstName: "Alice");

        var query = new GetAllUsersQuery(
            PageNumber: 1,
            PageSize: 10,
            SearchTerm: "doe"
        );

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2, "at least 2 users with 'doe' should exist");
        result.Items.Should().OnlyContain(u =>
            u.Email.Contains("doe", StringComparison.OrdinalIgnoreCase) ||
            u.FirstName.Contains("doe", StringComparison.OrdinalIgnoreCase) ||
            u.LastName.Contains("doe", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllUsers_WithInvalidPageSize_ShouldFail()
    {
        // Arrange
        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 150); // Max is 100

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("PageSize");
    }

    [Fact]
    public async Task GetAllUsers_SecondPage_ShouldReturnCorrectItems()
    {
        // Arrange - Create 5 users with unique emails
        for (int i = 1; i <= 5; i++)
        {
            await CreateTestUser();
        }

        var query = new GetAllUsersQuery(PageNumber: 2, PageSize: 2);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCountGreaterThanOrEqualTo(2, "second page should have at least 2 items");
        result.PageNumber.Should().Be(2);
    }
}
