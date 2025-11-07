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

            // Authentication
            "login" => new ParsedCommand { Type = CommandType.Login, Arguments = args },
            "logout" => new ParsedCommand { Type = CommandType.Logout },

            // Exchange Rates
            "current" => new ParsedCommand { Type = CommandType.GetCurrent, Arguments = args },
            "latest" => new ParsedCommand { Type = CommandType.GetLatest, Arguments = args },
            "historical" or "history" => new ParsedCommand { Type = CommandType.GetHistorical, Arguments = args },
            "convert" => new ParsedCommand { Type = CommandType.Convert, Arguments = args },

            // Currencies
            "currencies" or "curr" => new ParsedCommand { Type = CommandType.GetCurrencies, Arguments = args },
            "currency" => new ParsedCommand { Type = CommandType.GetCurrency, Arguments = args },

            // Providers
            "providers" or "prov" => new ParsedCommand { Type = CommandType.GetProviders, Arguments = args },
            "provider" => new ParsedCommand { Type = CommandType.GetProvider, Arguments = args },
            "provider-health" => new ParsedCommand { Type = CommandType.GetProviderHealth, Arguments = args },
            "provider-stats" => new ParsedCommand { Type = CommandType.GetProviderStats, Arguments = args },

            // Users (Admin only)
            "users" => new ParsedCommand { Type = CommandType.GetUsers, Arguments = args },
            "user" => new ParsedCommand { Type = CommandType.GetUser, Arguments = args },

            // Streaming
            "stream-start" or "stream" => new ParsedCommand { Type = CommandType.StartStreaming, Arguments = args },
            "stream-stop" => new ParsedCommand { Type = CommandType.StopStreaming },

            // Testing
            "compare" => new ParsedCommand { Type = CommandType.Compare, Arguments = args },
            "solo" => new ParsedCommand { Type = CommandType.Solo, Arguments = args },
            "test-all" => new ParsedCommand { Type = CommandType.TestAll, Arguments = args },

            _ => new ParsedCommand { Type = CommandType.Invalid, Arguments = new[] { input } }
        };
    }

    public static string[] GetAutoCompleteOptions()
    {
        return new[]
        {
            // General
            "help", "exit", "quit", "clear", "cls", "status",
            // Authentication
            "login", "logout",
            // Exchange Rates
            "current", "latest", "historical", "history", "convert",
            // Currencies
            "currencies", "curr", "currency",
            // Providers
            "providers", "prov", "provider", "provider-health", "provider-stats",
            // Users
            "users", "user",
            // Streaming
            "stream-start", "stream", "stream-stop",
            // Testing
            "compare", "solo", "test-all"
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

    // Authentication
    Login,
    Logout,

    // Exchange Rates
    GetCurrent,
    GetLatest,
    GetHistorical,
    Convert,

    // Currencies
    GetCurrencies,
    GetCurrency,

    // Providers
    GetProviders,
    GetProvider,
    GetProviderHealth,
    GetProviderStats,

    // Users
    GetUsers,
    GetUser,

    // Streaming
    StartStreaming,
    StopStreaming,

    // Testing
    Compare,
    Solo,
    TestAll
}
