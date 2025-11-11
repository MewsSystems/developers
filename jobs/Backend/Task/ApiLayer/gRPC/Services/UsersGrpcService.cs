using ApplicationLayer.Commands.Users.ChangeUserPassword;
using ApplicationLayer.Commands.Users.ChangeUserRole;
using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Commands.Users.DeleteUser;
using ApplicationLayer.Commands.Users.UpdateUserInfo;
using ApplicationLayer.Queries.Users.CheckEmailExists;
using ApplicationLayer.Queries.Users.GetAllUsers;
using ApplicationLayer.Queries.Users.GetUserByEmail;
using ApplicationLayer.Queries.Users.GetUserById;
using ApplicationLayer.Queries.Users.GetUsersByRole;
using DomainLayer.Enums;
using gRPC.Mappers;
using gRPC.Protos.Users;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace gRPC.Services;

/// <summary>
/// gRPC service for user management operations.
/// </summary>
[Authorize(Roles = "Admin")]
public class UsersGrpcService : UsersService.UsersServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersGrpcService> _logger;

    public UsersGrpcService(
        IMediator mediator,
        ILogger<UsersGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<GetAllUsersResponse> GetAllUsers(
        GetAllUsersRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetAllUsers request received");

        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query, context.CancellationToken);

        var response = new GetAllUsersResponse
        {
            Message = "Users retrieved successfully"
        };

        foreach (var user in users.Items)
        {
            response.Users.Add(new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Role = user.Role
            });
        }

        return response;
    }

    public override async Task<GetUserByIdResponse> GetUserById(
        GetUserByIdRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetUserById request: {Id}", request.Id);

        var query = new GetUserByIdQuery(request.Id);
        var user = await _mediator.Send(query, context.CancellationToken);

        if (user != null)
        {
            return new GetUserByIdResponse
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}".Trim(),
                    Role = user.Role
                }
            };
        }

        return new GetUserByIdResponse
        {
            Success = false,
            Message = $"User with ID {request.Id} not found",
            Error = CommonMappers.ToProtoError("NOT_FOUND", $"User with ID {request.Id} not found")
        };
    }

    public override async Task<GetUserByEmailResponse> GetUserByEmail(
        GetUserByEmailRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetUserByEmail request: {Email}", request.Email);

        var query = new GetUserByEmailQuery(request.Email);
        var user = await _mediator.Send(query, context.CancellationToken);

        if (user != null)
        {
            return new GetUserByEmailResponse
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}".Trim(),
                    Role = user.Role
                }
            };
        }

        return new GetUserByEmailResponse
        {
            Success = false,
            Message = $"User with email '{request.Email}' not found",
            Error = CommonMappers.ToProtoError("NOT_FOUND", $"User with email '{request.Email}' not found")
        };
    }

    public override async Task<GetUsersByRoleResponse> GetUsersByRole(
        GetUsersByRoleRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetUsersByRole request: {Role}", request.Role);

        // Parse Role string to UserRole enum
        if (!Enum.TryParse<UserRole>(request.Role, true, out var userRole))
        {
            return new GetUsersByRoleResponse
            {
                Message = $"Invalid role: {request.Role}. Valid values are: Consumer, Admin"
            };
        }

        var query = new GetUsersByRoleQuery(userRole);
        var users = await _mediator.Send(query, context.CancellationToken);

        var response = new GetUsersByRoleResponse
        {
            Message = $"Users with role '{request.Role}' retrieved successfully"
        };

        foreach (var user in users.Items)
        {
            response.Users.Add(new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Role = user.Role
            });
        }

        return response;
    }

    public override async Task<CheckEmailExistsResponse> CheckEmailExists(
        CheckEmailExistsRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("CheckEmailExists request: {Email}", request.Email);

        var query = new CheckEmailExistsQuery(request.Email);
        var exists = await _mediator.Send(query, context.CancellationToken);

        return new CheckEmailExistsResponse
        {
            Exists = exists,
            Message = exists
                ? $"Email '{request.Email}' is already registered"
                : $"Email '{request.Email}' is available"
        };
    }

    public override async Task<CreateUserResponse> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("CreateUser request: {Email}", request.Email);

        // Parse FullName into FirstName and LastName
        var nameParts = request.FullName?.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
        var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

        // Parse Role string to UserRole enum
        if (!Enum.TryParse<UserRole>(request.Role, true, out var userRole))
        {
            return new CreateUserResponse
            {
                Success = false,
                Message = $"Invalid role: {request.Role}. Valid values are: Consumer, Admin",
                Error = CommonMappers.ToProtoError("INVALID_ROLE", $"Invalid role: {request.Role}")
            };
        }

        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            firstName,
            lastName,
            userRole);

        var result = await _mediator.Send(command, context.CancellationToken);

        if (result.IsSuccess && result.Value > 0)
        {
            // Query for the created user
            var query = new GetUserByIdQuery(result.Value);
            var user = await _mediator.Send(query, context.CancellationToken);

            if (user != null)
            {
                return new CreateUserResponse
                {
                    Success = true,
                    Message = $"User {request.Email} created successfully",
                    Data = new UserInfo
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = $"{user.FirstName} {user.LastName}".Trim(),
                        Role = user.Role
                    }
                };
            }
        }

        return new CreateUserResponse
        {
            Success = false,
            Message = result.Error ?? "Failed to create user",
            Error = CommonMappers.ToProtoError("CREATE_ERROR", result.Error ?? "Failed to create user")
        };
    }

    public override async Task<UpdateUserInfoResponse> UpdateUserInfo(
        UpdateUserInfoRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("UpdateUserInfo request: {Id}", request.Id);

        // Parse FullName into FirstName and LastName
        var nameParts = request.FullName?.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
        var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

        var command = new UpdateUserInfoCommand(
            request.Id,
            firstName,
            lastName,
            request.Email);

        var result = await _mediator.Send(command, context.CancellationToken);

        return new UpdateUserInfoResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? "User information updated successfully"
                : result.Error ?? "Failed to update user information",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("UPDATE_ERROR", result.Error ?? "Failed to update user information")
                : null
        };
    }

    public override async Task<ChangeUserPasswordResponse> ChangeUserPassword(
        ChangeUserPasswordRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("ChangeUserPassword request: {Id}", request.Id);

        var command = new ChangeUserPasswordCommand(
            request.Id,
            request.CurrentPassword,
            request.NewPassword);

        var result = await _mediator.Send(command, context.CancellationToken);

        return new ChangeUserPasswordResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? "Password changed successfully"
                : result.Error ?? "Failed to change password",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("PASSWORD_ERROR", result.Error ?? "Failed to change password")
                : null
        };
    }

    public override async Task<ChangeUserRoleResponse> ChangeUserRole(
        ChangeUserRoleRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("ChangeUserRole request: {Id}, NewRole: {Role}", request.Id, request.NewRole);

        // Parse NewRole string to UserRole enum
        if (!Enum.TryParse<UserRole>(request.NewRole, true, out var newRole))
        {
            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = $"Invalid role: {request.NewRole}. Valid values are: Consumer, Admin",
                Error = CommonMappers.ToProtoError("INVALID_ROLE", $"Invalid role: {request.NewRole}")
            };
        }

        var command = new ChangeUserRoleCommand(request.Id, newRole);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new ChangeUserRoleResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"User role changed to '{request.NewRole}' successfully"
                : result.Error ?? "Failed to change user role",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("ROLE_ERROR", result.Error ?? "Failed to change user role")
                : null
        };
    }

    public override async Task<DeleteUserResponse> DeleteUser(
        DeleteUserRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("DeleteUser request: {Id}", request.Id);

        var command = new DeleteUserCommand(request.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new DeleteUserResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? "User deleted successfully"
                : result.Error ?? "Failed to delete user",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("DELETE_ERROR", result.Error ?? "Failed to delete user")
                : null
        };
    }
}
