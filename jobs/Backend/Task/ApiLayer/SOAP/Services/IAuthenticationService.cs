using System.ServiceModel;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for authentication operations.
/// </summary>
[ServiceContract(Namespace = "")]
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticate a user and generate JWT tokens.
    /// </summary>
    [OperationContract]
    Task<LoginResponse> LoginAsync(LoginRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for user login.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class LoginRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Response containing authentication result with JWT tokens.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class LoginResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public AuthenticationDataSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// SOAP model for authentication data.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class AuthenticationDataSoap
{
    [System.Runtime.Serialization.DataMember]
    public int UserId { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string Email { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string FirstName { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string LastName { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Role { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string AccessToken { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string RefreshToken { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public long ExpiresAt { get; set; }
}
