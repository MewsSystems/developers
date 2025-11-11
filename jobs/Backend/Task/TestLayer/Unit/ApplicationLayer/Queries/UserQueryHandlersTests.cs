using ApplicationLayer.Queries.Users.CheckEmailExists;
using ApplicationLayer.Queries.Users.GetAllUsers;
using ApplicationLayer.Queries.Users.GetUserByEmail;
using ApplicationLayer.Queries.Users.GetUserById;
using ApplicationLayer.Queries.Users.GetUsersByRole;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Enums;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for User query handlers.
/// </summary>
public class UserQueryHandlersTests : TestBase
{
    #region CheckEmailExistsQueryHandler Tests

    [Fact]
    public async Task CheckEmailExists_WhenEmailExists_ShouldReturnTrue()
    {
        // Arrange
        var handler = new CheckEmailExistsQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<CheckEmailExistsQueryHandler>().Object);

        var query = new CheckEmailExistsQuery("existing@example.com");

        MockUserRepository
            .Setup(x => x.ExistsByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        MockUserRepository.Verify(
            x => x.ExistsByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CheckEmailExists_WhenEmailDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var handler = new CheckEmailExistsQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<CheckEmailExistsQueryHandler>().Object);

        var query = new CheckEmailExistsQuery("nonexistent@example.com");

        MockUserRepository
            .Setup(x => x.ExistsByEmailAsync("nonexistent@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        MockUserRepository.Verify(
            x => x.ExistsByEmailAsync("nonexistent@example.com", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetAllUsersQueryHandler Tests

    [Fact]
    public async Task GetAllUsers_WithUsers_ShouldReturnPagedResult()
    {
        // Arrange
        var handler = new GetAllUsersQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetAllUsersQueryHandler>().Object);

        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 10);

        var users = new List<User>
        {
            User.Create("alice@example.com", "hash", "Alice", "Smith", UserRole.Consumer),
            User.Create("bob@example.com", "hash", "Bob", "Jones", UserRole.Consumer),
            User.Create("charlie@example.com", "hash", "Charlie", "Brown", UserRole.Admin)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Items.Should().Contain(u => u.Email == "alice@example.com");
        result.Items.Should().Contain(u => u.Email == "bob@example.com");
        result.Items.Should().Contain(u => u.Email == "charlie@example.com");
    }

    [Fact]
    public async Task GetAllUsers_WithNoUsers_ShouldReturnEmptyResult()
    {
        // Arrange
        var handler = new GetAllUsersQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetAllUsersQueryHandler>().Object);

        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 10);

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetAllUsers_WithRoleFilter_ShouldReturnOnlyMatchingUsers()
    {
        // Arrange
        var handler = new GetAllUsersQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetAllUsersQueryHandler>().Object);

        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 10, RoleFilter: UserRole.Admin);

        var users = new List<User>
        {
            User.Create("admin1@example.com", "hash", "Admin", "One", UserRole.Admin),
            User.Create("user@example.com", "hash", "Regular", "User", UserRole.Consumer),
            User.Create("admin2@example.com", "hash", "Admin", "Two", UserRole.Admin)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().AllSatisfy(u => u.Role.Should().Be("Admin"));
    }

    [Fact]
    public async Task GetAllUsers_WithSearchTerm_ShouldReturnMatchingUsers()
    {
        // Arrange
        var handler = new GetAllUsersQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetAllUsersQueryHandler>().Object);

        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 10, SearchTerm: "admin");

        var users = new List<User>
        {
            User.Create("admin@example.com", "hash", "John", "Admin", UserRole.Admin),
            User.Create("user@example.com", "hash", "Jane", "User", UserRole.Consumer),
            User.Create("superadmin@example.com", "hash", "Super", "Admin", UserRole.Admin)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().Contain(u => u.Email.Contains("admin"));
    }

    [Fact]
    public async Task GetAllUsers_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var handler = new GetAllUsersQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetAllUsersQueryHandler>().Object);

        var query = new GetAllUsersQuery(PageNumber: 2, PageSize: 2);

        var users = new List<User>
        {
            User.Create("alice@example.com", "hash", "Alice", "A", UserRole.Consumer),
            User.Create("bob@example.com", "hash", "Bob", "B", UserRole.Consumer),
            User.Create("charlie@example.com", "hash", "Charlie", "C", UserRole.Consumer),
            User.Create("dave@example.com", "hash", "Dave", "D", UserRole.Consumer),
            User.Create("eve@example.com", "hash", "Eve", "E", UserRole.Consumer)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(2);
        result.TotalPages.Should().Be(3);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    #endregion

    #region GetUserByEmailQueryHandler Tests

    [Fact]
    public async Task GetUserByEmail_WhenUserExists_ShouldReturnUserDto()
    {
        // Arrange
        var handler = new GetUserByEmailQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUserByEmailQueryHandler>().Object);

        var query = new GetUserByEmailQuery("user@example.com");

        var user = User.Create("user@example.com", "hash", "John", "Doe", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("user@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.FullName.Should().Be("John Doe");
        result.Role.Should().Be("Consumer");
    }

    [Fact]
    public async Task GetUserByEmail_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetUserByEmailQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUserByEmailQueryHandler>().Object);

        var query = new GetUserByEmailQuery("nonexistent@example.com");

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("nonexistent@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetUserByIdQueryHandler Tests

    [Fact]
    public async Task GetUserById_WhenUserExists_ShouldReturnUserDetailDto()
    {
        // Arrange
        var handler = new GetUserByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUserByIdQueryHandler>().Object);

        var query = new GetUserByIdQuery(UserId: 1);

        var user = User.Create("user@example.com", "hash", "Jane", "Smith", UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@example.com");
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
        result.FullName.Should().Be("Jane Smith");
        result.Role.Should().Be("Consumer");
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetUserByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUserByIdQueryHandler>().Object);

        var query = new GetUserByIdQuery(UserId: 999);

        MockUserRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetUsersByRoleQueryHandler Tests

    [Fact]
    public async Task GetUsersByRole_WithUsersMatchingRole_ShouldReturnPagedResult()
    {
        // Arrange
        var handler = new GetUsersByRoleQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUsersByRoleQueryHandler>().Object);

        var query = new GetUsersByRoleQuery(Role: UserRole.Admin, PageNumber: 1, PageSize: 10);

        var users = new List<User>
        {
            User.Create("admin1@example.com", "hash", "Admin", "One", UserRole.Admin),
            User.Create("user@example.com", "hash", "Regular", "User", UserRole.Consumer),
            User.Create("admin2@example.com", "hash", "Admin", "Two", UserRole.Admin),
            User.Create("manager@example.com", "hash", "Consumer", "User", UserRole.Consumer)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().AllSatisfy(u => u.Role.Should().Be("Admin"));
    }

    [Fact]
    public async Task GetUsersByRole_WithNoUsersMatchingRole_ShouldReturnEmptyResult()
    {
        // Arrange
        var handler = new GetUsersByRoleQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUsersByRoleQueryHandler>().Object);

        var query = new GetUsersByRoleQuery(Role: UserRole.Admin, PageNumber: 1, PageSize: 10);

        var users = new List<User>
        {
            User.Create("user@example.com", "hash", "Regular", "User", UserRole.Consumer),
            User.Create("manager@example.com", "hash", "Consumer", "User", UserRole.Consumer)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetUsersByRole_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var handler = new GetUsersByRoleQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetUsersByRoleQueryHandler>().Object);

        var query = new GetUsersByRoleQuery(Role: UserRole.Consumer, PageNumber: 2, PageSize: 2);

        var users = new List<User>
        {
            User.Create("consumer1@example.com", "hash", "Consumer", "One", UserRole.Consumer),
            User.Create("consumer2@example.com", "hash", "Consumer", "Two", UserRole.Consumer),
            User.Create("consumer3@example.com", "hash", "Consumer", "Three", UserRole.Consumer),
            User.Create("consumer4@example.com", "hash", "Consumer", "Four", UserRole.Consumer),
            User.Create("consumer5@example.com", "hash", "Consumer", "Five", UserRole.Consumer),
            User.Create("admin@example.com", "hash", "Admin", "User", UserRole.Admin)
        };

        MockUserRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(2);
        result.TotalPages.Should().Be(3);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    #endregion
}
