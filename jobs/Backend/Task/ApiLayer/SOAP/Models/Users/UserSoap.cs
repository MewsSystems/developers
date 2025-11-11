using System.Runtime.Serialization;

namespace SOAP.Models.Users;

/// <summary>
/// SOAP model for user information.
/// </summary>
[DataContract(Namespace = "")]
public class UserSoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Email { get; set; } = string.Empty;

    [DataMember]
    public string FirstName { get; set; } = string.Empty;

    [DataMember]
    public string LastName { get; set; } = string.Empty;

    [DataMember]
    public string FullName { get; set; } = string.Empty;

    [DataMember]
    public string Role { get; set; } = string.Empty;

    [DataMember]
    public bool IsActive { get; set; }

    [DataMember]
    public string CreatedAt { get; set; } = string.Empty;
}
