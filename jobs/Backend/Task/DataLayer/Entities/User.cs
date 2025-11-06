namespace DataLayer.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<SystemConfiguration> ModifiedConfigurations { get; set; } = new List<SystemConfiguration>();
    public ICollection<ExchangeRateFetchLog> FetchLogs { get; set; } = new List<ExchangeRateFetchLog>();
    public ICollection<ErrorLog> ErrorLogs { get; set; } = new List<ErrorLog>();
}
