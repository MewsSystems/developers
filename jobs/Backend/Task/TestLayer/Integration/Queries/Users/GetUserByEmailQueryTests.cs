using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Queries.Users.GetUserByEmail;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Users;

public class GetUserByEmailQueryTests : IntegrationTestBase
{
    private async Task<string> CreateTestUser(UserRole role = UserRole.Consumer)
    {
        var email = $"user_{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            Email: email,
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: role
        );

        await Mediator.Send(command);
        return email;
    }

    [Fact]
    public async Task GetUserByEmail_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var email = await CreateTestUser(UserRole.Consumer);

        var query = new GetUserByEmailQuery(email);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(email);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Role.Should().Be("Consumer");
    }

    [Fact]
    public async Task GetUserByEmail_WithNonExistentEmail_ShouldReturnNull()
    {
        // Arrange
        var query = new GetUserByEmailQuery($"nonexistent_{Guid.NewGuid():N}@example.com");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByEmail_WithAdminUser_ShouldReturnAdmin()
    {
        // Arrange
        var email = await CreateTestUser(UserRole.Admin);

        var query = new GetUserByEmailQuery(email);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(email);
        result.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task GetUserByEmail_IsCaseInsensitive_ShouldReturnUser()
    {
        // Arrange
        var email = await CreateTestUser();
        var upperCaseEmail = email.ToUpper();

        var query = new GetUserByEmailQuery(upperCaseEmail);

        // Act
        var result = await Mediator.Send(query);

        // Assert - Email search should be case-insensitive
        result.Should().NotBeNull();
        result!.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetUserByEmail_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var query = new GetUserByEmailQuery("invalid-email");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("Email");
    }
}
