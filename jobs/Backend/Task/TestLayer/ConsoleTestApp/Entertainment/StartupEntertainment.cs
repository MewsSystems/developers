using Spectre.Console;
using ConsoleTestApp.Config;

namespace ConsoleTestApp.Entertainment;

public class StartupEntertainment
{
    private readonly EntertainmentSettings _settings;
    private readonly Random _random = new();

    public StartupEntertainment(EntertainmentSettings settings)
    {
        _settings = settings;
    }

    public async Task ShowAsync(CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            AnsiConsole.MarkupLine("[yellow]Startup entertainment disabled. Waiting for APIs...[/]");
            return;
        }

        AnsiConsole.Clear();
        DisplayHeader();

        var startTime = DateTime.UtcNow;
        var endTime = startTime.AddSeconds(_settings.StartupDurationSeconds);
        var factIndex = 0;

        while (DateTime.UtcNow < endTime && !cancellationToken.IsCancellationRequested)
        {
            var remaining = endTime - DateTime.UtcNow;
            var elapsed = DateTime.UtcNow - startTime;
            var progressPercentage = (int)((elapsed.TotalSeconds / _settings.StartupDurationSeconds) * 100);

            // Display progress bar
            DisplayProgressBar(progressPercentage, remaining);

            // Display current fact
            var fact = Facts.GetRandomFact(_random);
            DisplayFact(factIndex + 1, fact);

            // Display API status (simulated)
            DisplayApiStatus(elapsed.TotalSeconds);

            // Display countdown
            DisplayCountdown(remaining);

            // Wait for next update
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_settings.FactIntervalSeconds), cancellationToken);
                factIndex++;

                // Clear for next update (except header)
                Console.SetCursorPosition(0, 8);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }

        // Show completion
        if (!cancellationToken.IsCancellationRequested)
        {
            DisplayCompletion();
        }
    }

    private static void DisplayHeader()
    {
        var rule = new Rule("[bold cyan]Exchange Rate API - Multi-Protocol Test Console[/]");
        rule.Justification = Justify.Center;
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();

        var panel = new Panel("[yellow]⏳ Initializing APIs and fetching historical data...[/]")
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Yellow)
        };
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    private static void DisplayProgressBar(int percentage, TimeSpan remaining)
    {
        var progressRule = new Rule($"[grey]Progress: {percentage}%[/]");
        progressRule.Justification = Justify.Left;
        AnsiConsole.Write(progressRule);

        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn()
            })
            .Start(ctx =>
            {
                var task = ctx.AddTask("[green]Startup[/]");
                task.Value = percentage;
                task.MaxValue = 100;
            });
    }

    private static void DisplayFact(int factNumber, string fact)
    {
        AnsiConsole.WriteLine();
        var factPanel = new Panel($"[bold white]{fact}[/]")
        {
            Header = new PanelHeader($"[cyan]💡 DID YOU KNOW? (Fact #{factNumber})[/]"),
            Border = BoxBorder.Double,
            BorderStyle = new Style(foreground: Spectre.Console.Color.Cyan1),
            Padding = new Padding(2, 1)
        };
        AnsiConsole.Write(factPanel);
        AnsiConsole.WriteLine();
    }

    private static void DisplayApiStatus(double elapsedSeconds)
    {
        var table = new Table()
        {
            Border = TableBorder.Rounded,
            BorderStyle = new Style(Color.Grey)
        };

        table.AddColumn(new TableColumn("[bold]API[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Status[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Details[/]"));

        // Simulate progressive startup
        var restReady = elapsedSeconds > 5;
        var soapReady = elapsedSeconds > 8;
        var grpcReady = elapsedSeconds > 6;
        var historicalProgress = Math.Min(100, (int)((elapsedSeconds / 60) * 100)); // Simulate 60s for historical fetch

        table.AddRow(
            "[cyan]REST[/]",
            restReady ? "[green]✓ Ready[/]" : "[yellow]⏳ Starting...[/]",
            restReady ? "http://localhost:5188" : "Initializing..."
        );

        table.AddRow(
            "[magenta]SOAP[/]",
            soapReady ? "[green]✓ Ready[/]" : "[yellow]⏳ Starting...[/]",
            soapReady ? "http://localhost:5002" : "Initializing..."
        );

        table.AddRow(
            "[blue]gRPC[/]",
            grpcReady ? "[green]✓ Ready[/]" : "[yellow]⏳ Starting...[/]",
            grpcReady ? "http://localhost:5001" : "Initializing..."
        );

        table.AddRow(
            "[yellow]Historical Data[/]",
            historicalProgress >= 100 ? "[green]✓ Complete[/]" : $"[yellow]⏳ {historicalProgress}%[/]",
            historicalProgress >= 100 ? "All providers synced" : $"Fetching rates... {historicalProgress}%"
        );

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private void DisplayCountdown(TimeSpan remaining)
    {
        var minutes = (int)remaining.TotalMinutes;
        var seconds = remaining.Seconds;

        AnsiConsole.MarkupLine($"[grey]⏱️  Next fact in: {_settings.FactIntervalSeconds} seconds | Total remaining: {minutes:D2}:{seconds:D2}[/]");
        AnsiConsole.WriteLine();
    }

    private static void DisplayCompletion()
    {
        AnsiConsole.Clear();

        var completionPanel = new Panel("[bold green]✓ All APIs are ready![/]\n\n" +
                                       "[white]Historical data has been fetched and cached.[/]\n" +
                                       "[white]You can now test all three protocols side-by-side.[/]")
        {
            Header = new PanelHeader("[green]🚀 Initialization Complete[/]"),
            Border = BoxBorder.Double,
            BorderStyle = new Style(Color.Green),
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(completionPanel);
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}
