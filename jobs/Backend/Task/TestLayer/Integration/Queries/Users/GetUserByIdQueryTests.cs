using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Queries.Users.GetUserById;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Users;

public class GetUserByIdQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestUser(string email = "test@example.com", string firstName = "John", string lastName = "Doe")
    {
        var command = new CreateUserCommand(
            Email: email,
            Password: "Test123!@#",
            FirstName: firstName,
            LastName: lastName,
            Role: UserRole.Consumer
        );

        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task GetUserById_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var userId = await CreateTestUser("test@example.com", "John", "Doe");

        var query = new GetUserByIdQuery(userId);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.FullName.Should().Be("John Doe");
        result.Role.Should().Be("Consumer");
    }

    [Fact]
    public async Task GetUserById_WithNonExistentUser_ShouldReturnNull()
    {
        // Arrange
        var query = new GetUserByIdQuery(999);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ShouldFail()
    {
        // Arrange
        var query = new GetUserByIdQuery(0);

        // Act & Assert
        // Should fail validation
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("UserId");
    }
}
