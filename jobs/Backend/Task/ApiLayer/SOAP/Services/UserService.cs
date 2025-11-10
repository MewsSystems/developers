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
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SOAP.Converters;

namespace SOAP.Services;

/// <summary>
/// SOAP service implementation for user operations.
/// Reuses ApplicationLayer commands/queries via MediatR.
/// </summary>
[Authorize(Roles = "Admin")]
public class UserService : IUserService
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IMediator mediator,
        ILogger<UserService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetAllUsersResponse> GetAllUsersAsync(GetAllUsersRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetAllUsers called with RoleFilter: {RoleFilter}", request.RoleFilter);

            // Convert to enum if role filter is provided
            DomainLayer.Enums.UserRole? roleFilter = null;
            if (!string.IsNullOrEmpty(request.RoleFilter) &&
                Enum.TryParse<DomainLayer.Enums.UserRole>(request.RoleFilter, ignoreCase: true, out var parsedRole))
            {
                roleFilter = parsedRole;
            }

            var query = new GetAllUsersQuery(PageNumber: 1, PageSize: 100, SearchTerm: null, RoleFilter: roleFilter);
            var pagedResult = await _mediator.Send(query);

            return new GetAllUsersResponse
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = pagedResult.Items.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllUsers SOAP operation");

            return new GetAllUsersResponse
            {
                Success = false,
                Message = "An error occurred while retrieving users",
                Data = Array.Empty<Models.Users.UserSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetUserById called for ID: {Id}", request.Id);

            var query = new GetUserByIdQuery(request.Id);
            var user = await _mediator.Send(query);

            if (user != null)
            {
                return new GetUserByIdResponse
                {
                    Success = true,
                    Message = $"User {request.Id} retrieved successfully",
                    Data = user.ToSoap()
                };
            }

            return new GetUserByIdResponse
            {
                Success = false,
                Message = $"User with ID {request.Id} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"User with ID {request.Id} not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserById SOAP operation");

            return new GetUserByIdResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Consumer,Admin")]
    public async Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetUserByEmail called for email: {Email}", request.Email);

            var query = new GetUserByEmailQuery(request.Email);
            var user = await _mediator.Send(query);

            if (user != null)
            {
                return new GetUserByEmailResponse
                {
                    Success = true,
                    Message = $"User with email '{request.Email}' retrieved successfully",
                    Data = user.ToSoap()
                };
            }

            return new GetUserByEmailResponse
            {
                Success = false,
                Message = $"User with email '{request.Email}' not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"User with email '{request.Email}' not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserByEmail SOAP operation");

            return new GetUserByEmailResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: CreateUser called for email: {Email}", request.Email);

            // Parse role string to enum
            if (!Enum.TryParse<DomainLayer.Enums.UserRole>(request.Role, ignoreCase: true, out var userRole))
            {
                return new CreateUserResponse
                {
                    Success = false,
                    Message = $"Invalid role: {request.Role}. Valid roles are: Admin, Consumer",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = $"Invalid role: {request.Role}"
                    }
                };
            }

            var command = new CreateUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                userRole
            );

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                // Result<int> returns the user ID, so we need to fetch the user to return it
                var getUserQuery = new GetUserByIdQuery(result.Value);
                var user = await _mediator.Send(getUserQuery);

                return new CreateUserResponse
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = user?.ToSoap()
                };
            }

            return new CreateUserResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUser SOAP operation");

            return new CreateUserResponse
            {
                Success = false,
                Message = "An error occurred while creating user",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: DeleteUser called for ID: {Id}", request.Id);

            var command = new DeleteUserCommand(request.Id);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new DeleteUserResponse
                {
                    Success = true,
                    Message = "User deleted successfully"
                };
            }

            return new DeleteUserResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteUser SOAP operation");

            return new DeleteUserResponse
            {
                Success = false,
                Message = "An error occurred while deleting user",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<ChangeUserRoleResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: ChangeUserRole called for UserId: {UserId}, NewRole: {NewRole}",
                request.UserId, request.NewRole);

            // Parse role string to enum
            if (!Enum.TryParse<DomainLayer.Enums.UserRole>(request.NewRole, ignoreCase: true, out var newRole))
            {
                return new ChangeUserRoleResponse
                {
                    Success = false,
                    Message = $"Invalid role: {request.NewRole}. Valid roles are: Admin, Consumer",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = $"Invalid role: {request.NewRole}"
                    }
                };
            }

            var command = new ChangeUserRoleCommand(request.UserId, newRole);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new ChangeUserRoleResponse
                {
                    Success = true,
                    Message = "User role changed successfully"
                };
            }

            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ChangeUserRole SOAP operation");

            return new ChangeUserRoleResponse
            {
                Success = false,
                Message = "An error occurred while changing user role",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Consumer,Admin")]
    public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: UpdateUser called for UserId: {UserId}", request.UserId);

            var command = new UpdateUserInfoCommand(
                request.UserId,
                request.FirstName,
                request.LastName,
                request.Email
            );

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new UpdateUserResponse
                {
                    Success = true,
                    Message = "User updated successfully"
                };
            }

            return new UpdateUserResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateUser SOAP operation");

            return new UpdateUserResponse
            {
                Success = false,
                Message = "An error occurred while updating user",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Consumer,Admin")]
    public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: ChangePassword called for UserId: {UserId}", request.UserId);

            var command = new ChangeUserPasswordCommand(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword
            );

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new ChangePasswordResponse
                {
                    Success = true,
                    Message = "Password changed successfully"
                };
            }

            return new ChangePasswordResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ChangePassword SOAP operation");

            return new ChangePasswordResponse
            {
                Success = false,
                Message = "An error occurred while changing password",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<CheckEmailExistsResponse> CheckEmailExistsAsync(CheckEmailExistsRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: CheckEmailExists called for email: {Email}", request.Email);

            var query = new CheckEmailExistsQuery(request.Email);
            var exists = await _mediator.Send(query);

            return new CheckEmailExistsResponse
            {
                Success = true,
                Message = exists ? "Email is already registered" : "Email is available",
                Exists = exists
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CheckEmailExists SOAP operation");

            return new CheckEmailExistsResponse
            {
                Success = false,
                Message = "An error occurred while checking email",
                Exists = false,
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<GetUsersByRoleResponse> GetUsersByRoleAsync(GetUsersByRoleRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetUsersByRole called for role: {Role}", request.Role);

            // Parse role string to enum
            if (!Enum.TryParse<DomainLayer.Enums.UserRole>(request.Role, ignoreCase: true, out var roleEnum))
            {
                return new GetUsersByRoleResponse
                {
                    Success = false,
                    Message = $"Invalid role: {request.Role}. Valid roles are: Admin, Consumer",
                    Data = Array.Empty<Models.Users.UserSoap>(),
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = $"Invalid role: {request.Role}"
                    }
                };
            }

            var query = new GetUsersByRoleQuery(roleEnum, 1, 1000);
            var pagedResult = await _mediator.Send(query);

            return new GetUsersByRoleResponse
            {
                Success = true,
                Message = $"Users with role '{request.Role}' retrieved successfully",
                Data = pagedResult.Items.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUsersByRole SOAP operation");

            return new GetUsersByRoleResponse
            {
                Success = false,
                Message = "An error occurred while retrieving users by role",
                Data = Array.Empty<Models.Users.UserSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }
}
