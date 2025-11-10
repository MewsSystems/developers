using ConsoleTestApp.Models;
using Spectre.Console;

namespace ConsoleTestApp.UI;

/// <summary>
/// Utilities for displaying data in the console using Spectre.Console.
/// </summary>
public static class DisplayUtilities
{
    public static void ShowWelcome()
    {
        AnsiConsole.Clear();

        var rule = new Rule("[bold cyan1]Exchange Rate API Test Console[/]");
        rule.Justification = Justify.Center;
        AnsiConsole.Write(rule);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Test and compare REST, SOAP, and gRPC APIs side-by-side[/]");
        AnsiConsole.MarkupLine("[dim]Type 'help' for available commands[/]");
        AnsiConsole.WriteLine();
    }

    public static void ShowHelp()
    {
        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.AddColumn(new TableColumn("[bold]Command[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Description[/]"));
        table.AddColumn(new TableColumn("[bold]Examples[/]").Centered());

        // Authentication
        table.AddRow("[bold yellow]═══ AUTHENTICATION ═══[/]", "", "");
        table.AddRow(
            "[cyan1]login[/]",
            "Login to a specific protocol",
            "login rest\nlogin grpc admin@example.com Pass123!"
        );
        table.AddRow(
            "[cyan1]login-all[/]",
            "Login to all protocols at once",
            "login-all\nlogin-all user@example.com Pass123!"
        );
        table.AddRow(
            "[cyan1]logout[/]",
            "Logout from a specific protocol",
            "logout rest\nlogout grpc"
        );
        table.AddRow(
            "[cyan1]logout-all[/]",
            "Logout from all protocols",
            "logout-all"
        );

        table.AddEmptyRow();

        // Exchange Rates
        table.AddRow("[bold yellow]═══ EXCHANGE RATES ═══[/]", "", "");
        table.AddRow(
            "[green]current[/]",
            "Get current exchange rates",
            "current rest\ncurrent grpc"
        );
        table.AddRow(
            "[green]latest[/]",
            "Get latest exchange rates for a protocol",
            "latest rest\nlatest soap\nlatest grpc"
        );
        table.AddRow(
            "[green]historical[/]",
            "Get historical rates (7 days by default)",
            "historical rest\nhistorical grpc"
        );
        table.AddRow(
            "[green]convert[/]",
            "Convert currency amount",
            "convert rest EUR USD 100\nconvert soap GBP JPY 50"
        );

        table.AddEmptyRow();

        // Currencies
        table.AddRow("[bold yellow]═══ CURRENCIES ═══[/]", "", "");
        table.AddRow(
            "[cyan1]currencies[/]",
            "List all currencies (alias: curr)",
            "currencies rest\ncurr soap"
        );
        table.AddRow(
            "[cyan1]currency[/]",
            "Get specific currency by code",
            "currency rest EUR\ncurrency grpc USD"
        );

        table.AddEmptyRow();

        // Providers
        table.AddRow("[bold yellow]═══ PROVIDERS ═══[/]", "", "");
        table.AddRow(
            "[magenta1]providers[/]",
            "List all exchange rate providers (alias: prov)",
            "providers rest\nprov grpc"
        );
        table.AddRow(
            "[magenta1]provider[/]",
            "Get specific provider by code",
            "provider rest ECB\nprovider soap CNB"
        );
        table.AddRow(
            "[magenta1]provider-health[/]",
            "Check provider health status",
            "provider-health rest ECB"
        );
        table.AddRow(
            "[magenta1]provider-stats[/]",
            "Get provider statistics",
            "provider-stats rest CNB"
        );

        table.AddEmptyRow();

        // Users (Admin)
        table.AddRow("[bold yellow]═══ USERS (ADMIN) ═══[/]", "", "");
        table.AddRow(
            "[red1]users[/]",
            "List all users (requires admin)",
            "users rest\nusers soap"
        );
        table.AddRow(
            "[red1]user[/]",
            "Get specific user by ID (requires admin)",
            "user rest 1\nuser grpc 2"
        );

        table.AddEmptyRow();

        // Streaming
        table.AddRow("[bold yellow]═══ STREAMING ═══[/]", "", "");
        table.AddRow(
            "[yellow]stream / stream-start[/]",
            "Start real-time streaming for a protocol",
            "stream rest\nstream-start grpc"
        );
        table.AddRow(
            "[yellow]stream-stop[/]",
            "Stop current streaming",
            "stream-stop"
        );

        table.AddEmptyRow();

        // Testing & Comparison
        table.AddRow("[bold yellow]═══ TESTING & COMPARISON ═══[/]", "", "");
        table.AddRow(
            "[bold green]test-all[/]",
            "Test ALL endpoints for a protocol",
            "test-all rest\ntest-all grpc"
        );
        table.AddRow(
            "[magenta1]compare[/]",
            "Compare all 3 APIs side-by-side",
            "compare latest\ncompare historical"
        );
        table.AddRow(
            "[magenta1]solo[/]",
            "Test a single API in solo mode",
            "solo rest\nsolo soap\nsolo grpc"
        );
        table.AddRow(
            "[magenta1]exit-solo[/]",
            "Exit solo mode and return to normal mode (alias: normal)",
            "exit-solo\nnormal"
        );

        table.AddEmptyRow();

        // Utility
        table.AddRow("[bold yellow]═══ UTILITY ═══[/]", "", "");
        table.AddRow(
            "[blue]status[/]",
            "Show current connection status",
            "status"
        );
        table.AddRow(
            "[blue]clear[/]",
            "Clear the console screen",
            "clear"
        );
        table.AddRow(
            "[blue]help[/]",
            "Show this help message (aliases: ?, h)",
            "help"
        );
        table.AddRow(
            "[red]exit[/]",
            "Exit the application (aliases: quit, q)",
            "exit"
        );

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[bold cyan1]Command Pattern:[/] [yellow]<command> <protocol> [[arguments]][/]");
        AnsiConsole.MarkupLine("[dim]• Protocol can be: [cyan1]rest[/], [yellow]soap[/], or [green]grpc[/][/]");
        AnsiConsole.MarkupLine("[dim]• Most commands follow the pattern above with protocol as first argument[/]");
        AnsiConsole.MarkupLine("[dim]• Use [cyan1]login-all[/] and [cyan1]logout-all[/] to manage all protocols at once[/]");
        AnsiConsole.MarkupLine("[dim]• Use [blue]status[/] to see authentication status for all protocols[/]");
        AnsiConsole.WriteLine();
    }

    public static void ShowExchangeRateData(ExchangeRateData data, string protocol)
    {
        if (data.Providers.Count == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]No data received from {protocol}[/]");
            return;
        }

        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.Title = new TableTitle($"[bold]{protocol} - Exchange Rates[/]");
        table.AddColumn("[bold]Provider[/]");
        table.AddColumn("[bold]Base → Target[/]");
        table.AddColumn("[bold]Rate[/]");
        table.AddColumn("[bold]Valid Date[/]");

        foreach (var provider in data.Providers)
        {
            foreach (var baseCurrency in provider.BaseCurrencies)
            {
                foreach (var rate in baseCurrency.TargetRates) // Show all rates
                {
                    table.AddRow(
                        Markup.Escape(provider.ProviderName),
                        Markup.Escape($"{baseCurrency.CurrencyCode} → {rate.CurrencyCode}"),
                        $"[green]{rate.Rate:F4}[/] (×{rate.Multiplier})",
                        Markup.Escape(rate.ValidDate.ToString("yyyy-MM-dd"))
                    );
                }
            }
        }

        table.Caption = new TableTitle($"[dim]Total rates: {data.TotalRates} | Fetched: {data.FetchedAt:HH:mm:ss}[/]");
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void ShowMetrics(ApiCallMetrics metrics, string protocol)
    {
        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.Title = new TableTitle($"[bold]{protocol} - Performance Metrics[/]");
        table.AddColumn("[bold]Metric[/]");
        table.AddColumn("[bold]Value[/]");

        var statusColor = metrics.Success ? "green" : "red";
        table.AddRow("Status", $"[{statusColor}]{(metrics.Success ? "SUCCESS" : "FAILED")}[/]");
        table.AddRow("Response Time", $"[cyan1]{metrics.ResponseTimeMs}[/] ms");
        table.AddRow("Payload Size", $"[yellow]{metrics.PayloadSizeBytes:N0}[/] bytes");

        if (!string.IsNullOrEmpty(metrics.ErrorMessage))
        {
            table.AddRow("Error", $"[red]{metrics.ErrorMessage}[/]");
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void ShowComparisonMetrics(Dictionary<string, ApiCallMetrics> results)
    {
        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.Title = new TableTitle("[bold]API Performance Comparison[/]");
        table.AddColumn("[bold]Protocol[/]");
        table.AddColumn("[bold]Status[/]");
        table.AddColumn("[bold]Response Time[/]");
        table.AddColumn("[bold]Payload Size[/]");
        table.AddColumn("[bold]Winner[/]");

        // Find fastest and smallest payload
        var successfulResults = results.Where(r => r.Value.Success).ToList();
        string? fastestKey = null;
        string? smallestKey = null;

        if (successfulResults.Any())
        {
            fastestKey = successfulResults.MinBy(r => r.Value.ResponseTimeMs).Key;
            smallestKey = successfulResults.MinBy(r => r.Value.PayloadSizeBytes).Key;
        }

        foreach (var (protocol, metrics) in results)
        {
            var statusColor = metrics.Success ? "green" : "red";
            var statusText = metrics.Success ? "✓ SUCCESS" : "✗ FAILED";

            var isFastest = fastestKey == protocol;
            var isSmallest = smallestKey == protocol;

            var responseTime = metrics.Success
                ? $"{metrics.ResponseTimeMs} ms"
                : "-";

            var payloadSize = metrics.Success
                ? $"{metrics.PayloadSizeBytes:N0} bytes"
                : "-";

            var winner = "";
            if (isFastest) winner += "⚡ Fastest ";
            if (isSmallest) winner += "📦 Smallest";

            table.AddRow(
                $"[bold]{protocol}[/]",
                $"[{statusColor}]{statusText}[/]",
                isFastest ? $"[green bold]{responseTime}[/]" : responseTime,
                isSmallest ? $"[green bold]{payloadSize}[/]" : payloadSize,
                $"[yellow]{winner}[/]"
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]✗ Error: {message.EscapeMarkup()}[/]");
    }

    public static void ShowSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]✓ {message.EscapeMarkup()}[/]");
    }

    public static void ShowInfo(string message)
    {
        AnsiConsole.MarkupLine($"[cyan1]ℹ {message.EscapeMarkup()}[/]");
    }

    public static void ShowWarning(string message)
    {
        AnsiConsole.MarkupLine($"[yellow]⚠ {message.EscapeMarkup()}[/]");
    }

    public static void ShowStatus(string protocol, bool isAuthenticated, bool isStreaming)
    {
        var panel = new Panel(new Markup(
            $"[bold]Protocol:[/] {protocol}\n" +
            $"[bold]Authenticated:[/] {(isAuthenticated ? "[green]Yes[/]" : "[red]No[/]")}\n" +
            $"[bold]Streaming:[/] {(isStreaming ? "[green]Active[/]" : "[dim]Inactive[/]")}"
        ));
        panel.Header = new PanelHeader("[bold]Current Status[/]");
        panel.Border = BoxBorder.Rounded;

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }
}
