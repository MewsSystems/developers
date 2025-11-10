using System.ServiceModel;
using SOAP.Models.Users;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for user operations.
/// </summary>
[ServiceContract(Namespace = "")]
public interface IUserService
{
    /// <summary>
    /// Get all users (Admin only).
    /// </summary>
    [OperationContract]
    Task<GetAllUsersResponse> GetAllUsersAsync(GetAllUsersRequest request);

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [OperationContract]
    Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request);

    /// <summary>
    /// Get a user by email.
    /// </summary>
    [OperationContract]
    Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest request);

    /// <summary>
    /// Create a new user (Admin only).
    /// </summary>
    [OperationContract]
    Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);

    /// <summary>
    /// Delete a user (Admin only).
    /// </summary>
    [OperationContract]
    Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request);

    /// <summary>
    /// Change user role (Admin only).
    /// </summary>
    [OperationContract]
    Task<ChangeUserRoleResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);

    /// <summary>
    /// Update user profile information.
    /// </summary>
    [OperationContract]
    Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);

    /// <summary>
    /// Change user password.
    /// </summary>
    [OperationContract]
    Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);

    /// <summary>
    /// Check if an email address is already registered (Admin only).
    /// </summary>
    [OperationContract]
    Task<CheckEmailExistsResponse> CheckEmailExistsAsync(CheckEmailExistsRequest request);

    /// <summary>
    /// Get users filtered by role (Admin only).
    /// </summary>
    [OperationContract]
    Task<GetUsersByRoleResponse> GetUsersByRoleAsync(GetUsersByRoleRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all users.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllUsersRequest
{
    [System.Runtime.Serialization.DataMember]
    public string? RoleFilter { get; set; }
}

/// <summary>
/// Response containing all users.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllUsersResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public UserSoap[] Data { get; set; } = Array.Empty<UserSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting a user by ID.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetUserByIdRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response containing a single user.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetUserByIdResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public UserSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting a user by email.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetUserByEmailRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Response containing a single user.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetUserByEmailResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public UserSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for creating a new user.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CreateUserRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Password { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string FirstName { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string LastName { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// Response for user creation.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CreateUserResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public UserSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for deleting a user.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeleteUserRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response for user deletion.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeleteUserResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for changing user role.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ChangeUserRoleRequest
{
    [System.Runtime.Serialization.DataMember]
    public int UserId { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string NewRole { get; set; } = string.Empty;
}

/// <summary>
/// Response for role change.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ChangeUserRoleResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for updating user information.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class UpdateUserRequest
{
    [System.Runtime.Serialization.DataMember]
    public int UserId { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string FirstName { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string LastName { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Response for user update.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class UpdateUserResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for changing user password.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ChangePasswordRequest
{
    [System.Runtime.Serialization.DataMember]
    public int UserId { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string CurrentPassword { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// Response for password change.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ChangePasswordResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for checking if email exists.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CheckEmailExistsRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Response for email existence check.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CheckEmailExistsResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public bool Exists { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting users by role.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetUsersByRoleRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// Response containing users filtered by role.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetUsersByRoleResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public UserSoap[] Data { get; set; } = Array.Empty<UserSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
