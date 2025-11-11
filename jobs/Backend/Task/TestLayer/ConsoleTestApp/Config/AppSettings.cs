namespace ConsoleTestApp.Config;

public class AppSettings
{
    public ApiEndpoints ApiEndpoints { get; set; } = new();
    public TestCredentials TestCredentials { get; set; } = new();
    public EntertainmentSettings Entertainment { get; set; } = new();
    public DisplaySettings Display { get; set; } = new();
}

public class ApiEndpoints
{
    public string Rest { get; set; } = string.Empty;
    public string RestHub { get; set; } = string.Empty;
    public string Soap { get; set; } = string.Empty;
    public string Grpc { get; set; } = string.Empty;
}

public class TestCredentials
{
    public UserCredentials Admin { get; set; } = new();
    public UserCredentials Consumer { get; set; } = new();
}

public class UserCredentials
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class EntertainmentSettings
{
    public int StartupDurationSeconds { get; set; } = 300;
    public int FactIntervalSeconds { get; set; } = 5;
    public bool Enabled { get; set; } = true;
}

public class DisplaySettings
{
    public bool ShowResponseTimes { get; set; } = true;
    public bool ShowPayloadSizes { get; set; } = true;
    public bool ShowDataDiffs { get; set; } = true;
    public bool CompactMode { get; set; } = false;
}
