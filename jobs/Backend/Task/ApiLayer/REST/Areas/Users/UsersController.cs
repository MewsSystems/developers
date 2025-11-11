using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.Queries.Users.GetAllUsers;
using ApplicationLayer.Queries.Users.GetUserById;
using ApplicationLayer.Queries.Users.GetUserByEmail;
using ApplicationLayer.Queries.Users.CheckEmailExists;
using ApplicationLayer.Queries.Users.GetUsersByRole;
using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Commands.Users.UpdateUserInfo;
using ApplicationLayer.Commands.Users.ChangeUserPassword;
using ApplicationLayer.Commands.Users.ChangeUserRole;
using ApplicationLayer.Commands.Users.DeleteUser;
using DomainLayer.Enums;
using REST.Response.Models.Common;
using REST.Response.Models.Areas.Users;
using REST.Response.Converters;
using REST.Request.Models.Areas.Users;

namespace REST.Areas.Users;

/// <summary>
/// API endpoints for user management.
/// </summary>
[ApiController]
[Area("Users")]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 100);
        var pagedResult = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<UserResponse>>.Ok(
            pagedResult.Items.Select(u => u.ToResponse()),
            "Users retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    /// <param name="id">User ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserByIdQuery(id);
        var userDto = await _mediator.Send(query);

        if (userDto != null)
        {
            var response = ApiResponse<UserResponse>.Ok(
                userDto.ToResponse(),
                "User retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<UserResponse>.NotFound($"User with ID {id} not found"));
    }

    /// <summary>
    /// Get a user by email address.
    /// </summary>
    /// <param name="email">User email</param>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var query = new GetUserByEmailQuery(email);
        var userDto = await _mediator.Send(query);

        if (userDto != null)
        {
            var response = ApiResponse<UserResponse>.Ok(
                userDto.ToResponse(),
                "User retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<UserResponse>.NotFound($"User with email '{email}' not found"));
    }

    /// <summary>
    /// Check if a user with the given email exists (Admin only).
    /// </summary>
    /// <param name="email">Email address to check</param>
    [HttpGet("check-email/{email}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    public async Task<IActionResult> CheckEmailExists(string email)
    {
        var query = new CheckEmailExistsQuery(email);
        var exists = await _mediator.Send(query);

        var response = ApiResponse<bool>.Ok(
            exists,
            exists ? "Email is already registered" : "Email is available"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get users filtered by role.
    /// </summary>
    /// <param name="role">User role (Consumer or Admin)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    [HttpGet("by-role/{role}")]
    [ProducesResponseType(typeof(PagedResponse<UserResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetByRole(string role, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        // Parse role string to UserRole enum
        if (!Enum.TryParse<UserRole>(role, true, out var roleEnum))
        {
            return BadRequest(ApiResponse.Fail(
                $"Invalid role: {role}. Valid roles are: Consumer, Admin",
                400
            ));
        }

        var query = new GetUsersByRoleQuery(roleEnum, pageNumber, pageSize);
        var pagedResult = await _mediator.Send(query);

        var pagedResponse = PagedResponse<UserResponse>.Ok(
            pagedResult.Items.Select(u => u.ToResponse()),
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            "Users retrieved successfully"
        );

        return Ok(pagedResponse);
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        // Parse role string to UserRole enum
        if (!Enum.TryParse<UserRole>(request.Role, true, out var roleEnum))
        {
            return BadRequest(ApiResponse<UserResponse>.BadRequest(
                $"Invalid role: {request.Role}. Valid roles are: Consumer, Admin"
            ));
        }

        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            roleEnum
        );

        var result = await _mediator.Send(command);

        if (result.IsSuccess && result.Value > 0)
        {
            // Query for the created user to return full details
            var getUserQuery = new GetUserByIdQuery(result.Value);
            var userDto = await _mediator.Send(getUserQuery);

            if (userDto != null)
            {
                var response = ApiResponse<UserResponse>.Ok(
                    userDto.ToResponse(),
                    "User created successfully"
                );
                return CreatedAtAction(nameof(GetById), new { id = result.Value }, response);
            }
        }

        return BadRequest(ApiResponse<UserResponse>.BadRequest(
            result.Error ?? "Failed to create user"
        ));
    }

    /// <summary>
    /// Update user information.
    /// </summary>
    /// <param name="id">User ID</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserInfoCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email
        );

        var result = await _mediator.Send(command);
        return Ok(result.ToApiResponse("User updated successfully"));
    }

    /// <summary>
    /// Change user password.
    /// </summary>
    /// <param name="id">User ID</param>
    [HttpPost("{id}/change-password")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
    {
        var command = new ChangeUserPasswordCommand(
            id,
            request.CurrentPassword,
            request.NewPassword
        );

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result.ToApiResponse("Password changed successfully"))
            : BadRequest(result.ToApiResponse());
    }

    /// <summary>
    /// Change user role.
    /// </summary>
    /// <param name="id">User ID</param>
    [HttpPost("{id}/change-role")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] ChangeRoleRequest request)
    {
        // Parse role string to UserRole enum
        if (!Enum.TryParse<UserRole>(request.NewRole, true, out var roleEnum))
        {
            return BadRequest(ApiResponse.Fail(
                $"Invalid role: {request.NewRole}. Valid roles are: Consumer, Admin",
                400
            ));
        }

        var command = new ChangeUserRoleCommand(id, roleEnum);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.ToApiResponse("User role changed successfully"))
            : BadRequest(result.ToApiResponse());
    }

    /// <summary>
    /// Delete a user.
    /// </summary>
    /// <param name="id">User ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.ToApiResponse("User deleted successfully"))
            : NotFound(result.ToApiResponse());
    }
}
