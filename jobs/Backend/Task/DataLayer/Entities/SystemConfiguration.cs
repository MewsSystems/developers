namespace DataLayer.Entities;

public class SystemConfiguration
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DataType { get; set; } = "String";
    public DateTimeOffset Modified { get; set; }
    public int? ModifiedBy { get; set; }

    // Navigation properties
    public User? ModifiedByUser { get; set; }
}
