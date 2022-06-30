namespace Common.Base;

/// <summary>
///     Contains all available environment variables
/// </summary>
public static class EnvironmentVariables
{
    /// <summary>
    ///     Application environment variable for choosing appsettings.
    ///     If nothing is set the application will use Production environment as a default.
    ///     If wrong value is used then default appsettings will be used
    /// </summary>
    public const string AppEnvironment = "ASPNETCORE_ENVIRONMENT";

    /// <summary>
    ///     Contains file path to the log. Starting point of the path is application folder and the path must ends with file
    ///     name. e. g. "log.txt"
    /// </summary>
    public const string AppSerilogFilePath = "ASPNETCORE_SERILOG_FILE_PATH";
}