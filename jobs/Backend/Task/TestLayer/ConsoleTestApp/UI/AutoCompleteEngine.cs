using Spectre.Console;

namespace ConsoleTestApp.UI;

/// <summary>
/// Context-aware autocomplete engine for command input.
/// </summary>
public class AutoCompleteEngine
{
    private readonly string[] _commands;
    private readonly string[] _protocols = { "rest", "soap", "grpc" };

    public AutoCompleteEngine(string[] commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Get suggestions based on current input and cursor position.
    /// </summary>
    public string[] GetSuggestions(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            // No input - suggest all commands
            return _commands;
        }

        // Check if we're starting a new token (input ends with space)
        var endsWithSpace = input.EndsWith(" ");
        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            return _commands;
        }

        if (parts.Length == 1 && !endsWithSpace)
        {
            // First token - suggest matching commands
            var partial = parts[0].ToLowerInvariant();
            var matches = _commands.Where(c => c.StartsWith(partial, StringComparison.OrdinalIgnoreCase)).ToArray();
            return matches.Length > 0 ? matches : _commands;
        }

        var command = parts[0].ToLowerInvariant();

        // Commands that don't take protocol as first argument
        var noProtocolCommands = new[] { "help", "?", "h", "exit", "quit", "q", "clear", "cls", "status", "logout-all", "compare", "solo", "test-all", "stream-stop", "login-all" };

        // Calculate effective token position (where we're suggesting)
        var tokenPosition = parts.Length + (endsWithSpace ? 1 : 0);

        if (tokenPosition == 2)
        {
            // Second token (first argument)
            if (noProtocolCommands.Contains(command))
            {
                // These commands have specific second arguments
                return command switch
                {
                    "compare" => new[] { "latest", "historical" },
                    "solo" => _protocols,
                    "login-all" => new[] { "[[email]]" },
                    _ => Array.Empty<string>()
                };
            }

            // Most commands expect protocol as second argument
            if (endsWithSpace)
            {
                return _protocols;
            }
            else
            {
                var partial = parts[1].ToLowerInvariant();
                var matches = _protocols.Where(p => p.StartsWith(partial, StringComparison.OrdinalIgnoreCase)).ToArray();
                return matches.Length > 0 ? matches : _protocols;
            }
        }

        if (tokenPosition == 3)
        {
            // Third token - context depends on command
            return command switch
            {
                "login" => new[] { "[[email]]" },
                "login-all" => new[] { "[[password]]" },
                "currency" => new[] { "EUR", "USD", "GBP", "JPY", "CHF", "CZK", "RON" },
                "currency-id" => new[] { "1", "2", "3", "[[id]]" },
                "provider" => new[] { "ECB", "CNB", "BNR" },
                "provider-id" => new[] { "1", "2", "3", "[[id]]" },
                "provider-health" => new[] { "ECB", "CNB", "BNR" },
                "provider-stats" => new[] { "ECB", "CNB", "BNR" },
                "provider-config" => new[] { "ECB", "CNB", "BNR" },
                "activate-provider" => new[] { "ECB", "CNB", "BNR" },
                "deactivate-provider" => new[] { "ECB", "CNB", "BNR" },
                "reset-provider-health" => new[] { "ECB", "CNB", "BNR" },
                "trigger-fetch" or "manual-fetch" => new[] { "ECB", "CNB", "BNR" },
                "delete-provider" => new[] { "ECB", "CNB", "BNR" },
                "delete-currency" => new[] { "EUR", "USD", "GBP", "[[code]]" },
                "create-currency" => new[] { "EUR", "USD", "GBP", "[[code]]" },
                "reschedule-provider" or "provider-reschedule" => new[] { "ECB", "CNB", "BNR" },
                "user" => new[] { "1", "2", "3", "[[id]]" },
                "user-by-email" => new[] { "[[email]]" },
                "users-by-role" => new[] { "Admin", "Consumer" },
                "convert" => new[] { "EUR", "USD", "GBP", "JPY" },
                "latest-rate" => new[] { "EUR", "USD", "GBP" },
                "update-user" => new[] { "1", "2", "3", "[[id]]" },
                "change-password" => new[] { "1", "2", "3", "[[id]]" },
                "change-user-role" => new[] { "1", "2", "3", "[[id]]" },
                "delete-user" => new[] { "1", "2", "3", "[[id]]" },
                "errors" => new[] { "10", "20", "50", "[[count]]" },
                "activity" or "fetch-activity" => new[] { "10", "20", "50", "[[count]]" },
                "create-provider" => new[] { "[[name]]" },
                "update-provider-config" => new[] { "ECB", "CNB", "BNR" },
                _ => Array.Empty<string>()
            };
        }

        // For 4+ tokens, provide hints based on command structure
        if (tokenPosition >= 4)
        {
            return command switch
            {
                "convert" when tokenPosition == 4 => new[] { "EUR", "USD", "GBP", "JPY" },
                "convert" when tokenPosition == 5 => new[] { "100", "50", "1000", "[[amount]]" },
                "login" when tokenPosition == 4 => new[] { "[[password]]" },
                "latest-rate" when tokenPosition == 4 => new[] { "EUR", "USD", "GBP", "JPY" },
                "latest-rate" when tokenPosition == 5 => new[] { "1", "2", "3", "[[providerId]]" },
                "create-user" when tokenPosition == 4 => new[] { "[[password]]" },
                "create-user" when tokenPosition == 5 => new[] { "[[firstName]]" },
                "create-user" when tokenPosition == 6 => new[] { "[[lastName]]" },
                "create-user" when tokenPosition == 7 => new[] { "Admin", "Consumer" },
                "update-user" when tokenPosition == 4 => new[] { "[[firstName]]" },
                "update-user" when tokenPosition == 5 => new[] { "[[lastName]]" },
                "change-password" when tokenPosition == 4 => new[] { "[[currentPassword]]" },
                "change-password" when tokenPosition == 5 => new[] { "[[newPassword]]" },
                "change-user-role" when tokenPosition == 4 => new[] { "Admin", "Consumer" },
                "reschedule-provider" or "provider-reschedule" when tokenPosition == 4 => new[] { "14:30", "16:00", "[[HH:mm]]" },
                "reschedule-provider" or "provider-reschedule" when tokenPosition == 5 => new[] { "UTC", "CET", "EET" },
                "delete-provider" when tokenPosition == 4 => new[] { "true", "false" },
                "errors" when tokenPosition == 4 => new[] { "Error", "Warning", "Info", "[[severity]]" },
                "activity" or "fetch-activity" when tokenPosition == 4 => new[] { "1", "2", "3", "[[providerId]]" },
                "activity" or "fetch-activity" when tokenPosition == 5 => new[] { "true", "false", "[[failedOnly]]" },
                "create-provider" when tokenPosition == 4 => new[] { "ECB", "CNB", "BNR", "[[code]]" },
                "create-provider" when tokenPosition == 5 => new[] { "[[url]]" },
                "create-provider" when tokenPosition == 6 => new[] { "1", "2", "3", "[[baseCurrencyId]]" },
                "create-provider" when tokenPosition == 7 => new[] { "true", "false", "[[requiresAuth]]" },
                "create-provider" when tokenPosition == 8 => new[] { "[[apiKeyRef]]" },
                "update-provider-config" when tokenPosition == 4 => new[] { "[[name]]" },
                "update-provider-config" when tokenPosition == 5 => new[] { "[[url]]" },
                "update-provider-config" when tokenPosition == 6 => new[] { "true", "false", "[[requiresAuth]]" },
                "update-provider-config" when tokenPosition == 7 => new[] { "[[apiKeyRef]]" },
                _ => Array.Empty<string>()
            };
        }

        return Array.Empty<string>();
    }

    /// <summary>
    /// Get command usage hint.
    /// </summary>
    public string GetUsageHint(string command)
    {
        return command.ToLowerInvariant() switch
        {
            // Authentication
            "login" => "login <protocol> [[email]] [[password]]",
            "login-all" => "login-all [[email]] [[password]]",
            "logout" => "logout <protocol>",
            "logout-all" => "logout-all",

            // Exchange Rates
            "current" => "current <protocol>",
            "current-grouped" => "current-grouped <protocol>",
            "latest" => "latest <protocol>",
            "latest-rate" => "latest-rate <protocol> <source> <target> [[providerId]]",
            "historical" or "history" => "historical <protocol>",
            "convert" => "convert <protocol> <from> <to> <amount>",

            // Currencies
            "currencies" or "curr" => "currencies <protocol>",
            "currency" => "currency <protocol> <code>",
            "currency-id" => "currency-id <protocol> <id>",
            "create-currency" => "create-currency <protocol> <code>",
            "delete-currency" => "delete-currency <protocol> <code>",

            // Providers
            "providers" or "prov" => "providers <protocol>",
            "provider" => "provider <protocol> <code>",
            "provider-id" => "provider-id <protocol> <id>",
            "provider-health" => "provider-health <protocol> <code>",
            "provider-stats" => "provider-stats <protocol> <code>",
            "provider-config" => "provider-config <protocol> <code>",
            "activate-provider" => "activate-provider <protocol> <code>",
            "deactivate-provider" => "deactivate-provider <protocol> <code>",
            "reset-provider-health" => "reset-provider-health <protocol> <code>",
            "trigger-fetch" or "manual-fetch" => "trigger-fetch <protocol> <code>",
            "create-provider" => "create-provider <protocol> <name> <code> <url> <baseCurrencyId> <requiresAuth> [[apiKeyRef]]",
            "update-provider-config" => "update-provider-config <protocol> <code> <name> <url> <requiresAuth> [[apiKeyRef]]",
            "delete-provider" => "delete-provider <protocol> <code> <force>",
            "reschedule-provider" or "provider-reschedule" => "reschedule-provider <protocol> <code> <time:HH:mm> <timezone>",

            // Users
            "users" => "users <protocol>",
            "user" => "user <protocol> <id>",
            "user-by-email" => "user-by-email <protocol> <email>",
            "users-by-role" => "users-by-role <protocol> <role>",
            "check-email" => "check-email <protocol> <email>",
            "create-user" => "create-user <protocol> <email> <password> <firstName> <lastName> <role>",
            "update-user" => "update-user <protocol> <id> <firstName> <lastName>",
            "change-password" => "change-password <protocol> <id> <currentPassword> <newPassword>",
            "change-user-role" => "change-user-role <protocol> <id> <newRole>",
            "delete-user" => "delete-user <protocol> <id>",

            // System Health
            "system-health" or "health" => "system-health <protocol>",
            "errors" => "errors <protocol> [[count]] [[severity]]",
            "fetch-activity" or "activity" => "fetch-activity <protocol> [[count]] [[providerId]] [[failedOnly]]",

            // Testing & Comparison
            "compare" => "compare <latest|historical>",
            "solo" => "solo <protocol>",
            "test" => "test <protocol>",
            "test-all" => "test-all",

            // Streaming
            "stream" or "stream-start" => "stream <protocol>",
            "stream-stop" => "stream-stop",

            // Utility
            "status" => "status",
            "check-api" or "ping" => "check-api <protocol>",
            "help" or "?" or "h" => "help",
            "clear" or "cls" => "clear",
            "exit" or "quit" or "q" => "exit",

            _ => ""
        };
    }
}
