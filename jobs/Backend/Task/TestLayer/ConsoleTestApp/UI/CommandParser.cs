namespace ConsoleTestApp.UI;

/// <summary>
/// Parses user input into structured commands.
/// </summary>
public class CommandParser
{
    public static ParsedCommand Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new ParsedCommand { Type = CommandType.Invalid };
        }

        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLowerInvariant();
        var args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

        return command switch
        {
            // General
            "help" or "?" or "h" => new ParsedCommand { Type = CommandType.Help },
            "exit" or "quit" or "q" => new ParsedCommand { Type = CommandType.Exit },
            "clear" or "cls" => new ParsedCommand { Type = CommandType.Clear },
            "status" => new ParsedCommand { Type = CommandType.Status },
            "check-api" or "ping" => new ParsedCommand { Type = CommandType.IsApiAvailable, Arguments = args },

            // Authentication
            "login" => new ParsedCommand { Type = CommandType.Login, Arguments = args },
            "login-all" => new ParsedCommand { Type = CommandType.LoginAll, Arguments = args },
            "logout" => new ParsedCommand { Type = CommandType.Logout, Arguments = args },
            "logout-all" => new ParsedCommand { Type = CommandType.LogoutAll },

            // Exchange Rates
            "current" => new ParsedCommand { Type = CommandType.GetCurrent, Arguments = args },
            "current-grouped" => new ParsedCommand { Type = CommandType.GetCurrentGrouped, Arguments = args },
            "latest" => new ParsedCommand { Type = CommandType.GetLatest, Arguments = args },
            "latest-rate" => new ParsedCommand { Type = CommandType.GetLatestRate, Arguments = args },
            "historical" or "history" => new ParsedCommand { Type = CommandType.GetHistorical, Arguments = args },
            "convert" => new ParsedCommand { Type = CommandType.Convert, Arguments = args },

            // Currencies
            "currencies" or "curr" => new ParsedCommand { Type = CommandType.GetCurrencies, Arguments = args },
            "currency" => new ParsedCommand { Type = CommandType.GetCurrency, Arguments = args },
            "currency-id" => new ParsedCommand { Type = CommandType.GetCurrencyById, Arguments = args },
            "create-currency" => new ParsedCommand { Type = CommandType.CreateCurrency, Arguments = args },
            "delete-currency" => new ParsedCommand { Type = CommandType.DeleteCurrency, Arguments = args },

            // Providers
            "providers" or "prov" => new ParsedCommand { Type = CommandType.GetProviders, Arguments = args },
            "provider" => new ParsedCommand { Type = CommandType.GetProvider, Arguments = args },
            "provider-id" => new ParsedCommand { Type = CommandType.GetProviderById, Arguments = args },
            "provider-health" => new ParsedCommand { Type = CommandType.GetProviderHealth, Arguments = args },
            "provider-stats" => new ParsedCommand { Type = CommandType.GetProviderStats, Arguments = args },
            "provider-config" => new ParsedCommand { Type = CommandType.GetProviderConfiguration, Arguments = args },
            "activate-provider" => new ParsedCommand { Type = CommandType.ActivateProvider, Arguments = args },
            "deactivate-provider" => new ParsedCommand { Type = CommandType.DeactivateProvider, Arguments = args },
            "reset-provider-health" => new ParsedCommand { Type = CommandType.ResetProviderHealth, Arguments = args },
            "trigger-fetch" or "manual-fetch" => new ParsedCommand { Type = CommandType.TriggerManualFetch, Arguments = args },
            "create-provider" => new ParsedCommand { Type = CommandType.CreateProvider, Arguments = args },
            "update-provider-config" => new ParsedCommand { Type = CommandType.UpdateProviderConfiguration, Arguments = args },
            "delete-provider" => new ParsedCommand { Type = CommandType.DeleteProvider, Arguments = args },
            "reschedule-provider" or "provider-reschedule" => new ParsedCommand { Type = CommandType.RescheduleProvider, Arguments = args },

            // Users (Admin only)
            "users" => new ParsedCommand { Type = CommandType.GetUsers, Arguments = args },
            "user" => new ParsedCommand { Type = CommandType.GetUser, Arguments = args },
            "user-by-email" => new ParsedCommand { Type = CommandType.GetUserByEmail, Arguments = args },
            "users-by-role" => new ParsedCommand { Type = CommandType.GetUsersByRole, Arguments = args },
            "check-email" => new ParsedCommand { Type = CommandType.CheckEmailExists, Arguments = args },
            "create-user" => new ParsedCommand { Type = CommandType.CreateUser, Arguments = args },
            "update-user" => new ParsedCommand { Type = CommandType.UpdateUser, Arguments = args },
            "change-password" => new ParsedCommand { Type = CommandType.ChangePassword, Arguments = args },
            "change-user-role" => new ParsedCommand { Type = CommandType.ChangeUserRole, Arguments = args },
            "delete-user" => new ParsedCommand { Type = CommandType.DeleteUser, Arguments = args },

            // System Health (Admin only)
            "system-health" or "health" => new ParsedCommand { Type = CommandType.GetSystemHealth, Arguments = args },
            "errors" => new ParsedCommand { Type = CommandType.GetRecentErrors, Arguments = args },
            "fetch-activity" or "activity" => new ParsedCommand { Type = CommandType.GetFetchActivity, Arguments = args },

            // Streaming
            "stream-start" or "stream" => new ParsedCommand { Type = CommandType.StartStreaming, Arguments = args },
            "stream-stop" => new ParsedCommand { Type = CommandType.StopStreaming },

            // Testing
            "compare" => new ParsedCommand { Type = CommandType.Compare, Arguments = args },
            "solo" => new ParsedCommand { Type = CommandType.Solo, Arguments = args },
            "exit-solo" or "normal" => new ParsedCommand { Type = CommandType.ExitSolo },
            "test" => new ParsedCommand { Type = CommandType.Test, Arguments = args },
            "test-all" => new ParsedCommand { Type = CommandType.TestAll, Arguments = args },

            _ => new ParsedCommand { Type = CommandType.Invalid, Arguments = new[] { input } }
        };
    }

    public static string[] GetAutoCompleteOptions()
    {
        return new[]
        {
            // General
            "help", "exit", "quit", "clear", "cls", "status", "check-api", "ping",
            // Authentication
            "login", "login-all", "logout", "logout-all",
            // Exchange Rates
            "current", "current-grouped", "latest", "latest-rate", "historical", "history", "convert",
            // Currencies
            "currencies", "curr", "currency", "currency-id", "create-currency", "delete-currency",
            // Providers
            "providers", "prov", "provider", "provider-id", "provider-health", "provider-stats", "provider-config",
            "activate-provider", "deactivate-provider", "reset-provider-health", "trigger-fetch", "manual-fetch",
            "create-provider", "update-provider-config", "delete-provider", "reschedule-provider", "provider-reschedule",
            // Users
            "users", "user", "user-by-email", "users-by-role", "check-email",
            "create-user", "update-user", "change-password", "change-user-role", "delete-user",
            // System Health
            "system-health", "health", "errors", "fetch-activity", "activity",
            // Streaming
            "stream-start", "stream", "stream-stop",
            // Testing
            "compare", "solo", "exit-solo", "normal", "test", "test-all"
        };
    }
}

public class ParsedCommand
{
    public CommandType Type { get; set; }
    public string[] Arguments { get; set; } = Array.Empty<string>();
}

public enum CommandType
{
    Invalid,
    Help,
    Exit,
    Clear,
    Status,
    IsApiAvailable,

    // Authentication
    Login,
    LoginAll,
    Logout,
    LogoutAll,

    // Exchange Rates
    GetCurrent,
    GetCurrentGrouped,
    GetLatest,
    GetLatestRate,
    GetHistorical,
    Convert,

    // Currencies
    GetCurrencies,
    GetCurrency,
    GetCurrencyById,
    CreateCurrency,
    DeleteCurrency,

    // Providers
    GetProviders,
    GetProvider,
    GetProviderById,
    GetProviderHealth,
    GetProviderStats,
    GetProviderConfiguration,
    ActivateProvider,
    DeactivateProvider,
    ResetProviderHealth,
    TriggerManualFetch,
    CreateProvider,
    UpdateProviderConfiguration,
    DeleteProvider,
    RescheduleProvider,

    // Users
    GetUsers,
    GetUser,
    GetUserByEmail,
    GetUsersByRole,
    CheckEmailExists,
    CreateUser,
    UpdateUser,
    ChangePassword,
    ChangeUserRole,
    DeleteUser,

    // System Health
    GetSystemHealth,
    GetRecentErrors,
    GetFetchActivity,

    // Streaming
    StartStreaming,
    StopStreaming,

    // Testing
    Compare,
    Solo,
    ExitSolo,
    Test,
    TestAll
}
