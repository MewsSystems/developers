using System.ServiceModel;
using SOAP.Models.Users;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for user operations.
/// </summary>
[ServiceContract]
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
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all users.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetAllUsersRequest
{
    [System.Runtime.Serialization.DataMember]
    public string? RoleFilter { get; set; }
}

/// <summary>
/// Response containing all users.
/// </summary>
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
public class GetUserByIdRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response containing a single user.
/// </summary>
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
public class GetUserByEmailRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Response containing a single user.
/// </summary>
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
public class DeleteUserRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response for user deletion.
/// </summary>
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
public class ChangeUserRoleResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
