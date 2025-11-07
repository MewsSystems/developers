using ConsoleTestApp.Config;
using ConsoleTestApp.Core;
using ConsoleTestApp.Models;
using Spectre.Console;

namespace ConsoleTestApp.UI;

/// <summary>
/// Main interactive console controller that handles user commands and API testing.
/// </summary>
public class InteractiveConsole
{
    private readonly ApiClientFactory _factory;
    private readonly TestCredentials _credentials;
    private readonly CancellationTokenSource _cts = new();

    private IApiClient? _currentClient;
    private ApiProtocol? _currentProtocol;
    private bool _isStreaming = false;

    public InteractiveConsole(AppSettings settings)
    {
        _factory = new ApiClientFactory(settings.ApiEndpoints);
        _credentials = settings.TestCredentials;
    }

    public async Task RunAsync()
    {
        DisplayUtilities.ShowWelcome();

        while (!_cts.Token.IsCancellationRequested)
        {
            try
            {
                var input = AnsiConsole.Prompt(
                    new TextPrompt<string>("[bold cyan1]>[/]")
                        .AllowEmpty()
                );

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var command = CommandParser.Parse(input);
                await ExecuteCommandAsync(command);
            }
            catch (Exception ex)
            {
                DisplayUtilities.ShowError($"Unexpected error: {ex.Message}");
            }
        }
    }

    private async Task ExecuteCommandAsync(ParsedCommand command)
    {
        switch (command.Type)
        {
            case CommandType.Help:
                DisplayUtilities.ShowHelp();
                break;

            case CommandType.Exit:
                await CleanupAndExitAsync();
                break;

            case CommandType.Login:
                await HandleLoginAsync(command.Arguments);
                break;

            case CommandType.Logout:
                await HandleLogoutAsync();
                break;

            case CommandType.GetLatest:
                await HandleGetLatestAsync(command.Arguments);
                break;

            case CommandType.GetHistorical:
                await HandleGetHistoricalAsync(command.Arguments);
                break;

            case CommandType.StartStreaming:
                await HandleStartStreamingAsync(command.Arguments);
                break;

            case CommandType.StopStreaming:
                await HandleStopStreamingAsync();
                break;

            case CommandType.Compare:
                await HandleCompareAsync(command.Arguments);
                break;

            case CommandType.Solo:
                await HandleSoloAsync(command.Arguments);
                break;

            case CommandType.Status:
                HandleStatus();
                break;

            case CommandType.Clear:
                DisplayUtilities.ShowWelcome();
                break;

            case CommandType.GetCurrent:
                await HandleGetCurrentAsync(command.Arguments);
                break;

            case CommandType.Convert:
                await HandleConvertAsync(command.Arguments);
                break;

            case CommandType.GetCurrencies:
                await HandleGetCurrenciesAsync(command.Arguments);
                break;

            case CommandType.GetCurrency:
                await HandleGetCurrencyAsync(command.Arguments);
                break;

            case CommandType.GetProviders:
                await HandleGetProvidersAsync(command.Arguments);
                break;

            case CommandType.GetProvider:
                await HandleGetProviderAsync(command.Arguments);
                break;

            case CommandType.GetProviderHealth:
                await HandleGetProviderHealthAsync(command.Arguments);
                break;

            case CommandType.GetProviderStats:
                await HandleGetProviderStatsAsync(command.Arguments);
                break;

            case CommandType.GetUsers:
                await HandleGetUsersAsync(command.Arguments);
                break;

            case CommandType.GetUser:
                await HandleGetUserAsync(command.Arguments);
                break;

            case CommandType.TestAll:
                await HandleTestAllAsync(command.Arguments);
                break;

            case CommandType.Invalid:
                DisplayUtilities.ShowError($"Unknown command: {string.Join(" ", command.Arguments)}");
                DisplayUtilities.ShowInfo("Type 'help' for available commands");
                break;
        }
    }

    private async Task HandleLoginAsync(string[] args)
    {
        if (_currentClient == null)
        {
            DisplayUtilities.ShowError("No protocol selected. Use 'solo <protocol>' first");
            return;
        }

        string email, password;

        if (args.Length >= 2)
        {
            email = args[0];
            password = args[1];
        }
        else
        {
            // Use default admin credentials
            email = _credentials.Admin.Email;
            password = _credentials.Admin.Password;
            DisplayUtilities.ShowInfo($"Using default admin credentials: {email}");
        }

        await AnsiConsole.Status()
            .StartAsync("Logging in...", async ctx =>
            {
                var result = await _currentClient.LoginAsync(email, password);

                if (result.Success)
                {
                    DisplayUtilities.ShowSuccess($"Logged in as {result.Email} ({result.Role})");
                    DisplayUtilities.ShowInfo($"Token expires at: {result.ExpiresAt:yyyy-MM-dd HH:mm:ss}");
                }
                else
                {
                    DisplayUtilities.ShowError($"Login failed: {result.ErrorMessage}");
                }
            });
    }

    private async Task HandleLogoutAsync()
    {
        if (_currentClient == null)
        {
            DisplayUtilities.ShowWarning("No active session");
            return;
        }

        await _currentClient.LogoutAsync();
        DisplayUtilities.ShowSuccess("Logged out successfully");
    }

    private async Task HandleGetLatestAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: latest <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching latest rates from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetLatestRatesAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    DisplayUtilities.ShowExchangeRateData(data, protocol.ToString().ToUpper());
                }
            });
    }

    private async Task HandleGetHistoricalAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: historical <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);
        var from = DateTime.UtcNow.AddDays(-7);
        var to = DateTime.UtcNow;

        await AnsiConsole.Status()
            .StartAsync($"Fetching historical rates from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetHistoricalRatesAsync(from, to);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    DisplayUtilities.ShowExchangeRateData(data, protocol.ToString().ToUpper());
                }
            });
    }

    private async Task HandleStartStreamingAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: stream <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (_isStreaming)
        {
            DisplayUtilities.ShowWarning("Stopping current stream first...");
            await HandleStopStreamingAsync();
        }

        _currentClient = _factory.CreateClient(protocol);
        _currentProtocol = protocol;

        DisplayUtilities.ShowInfo($"Starting {protocol.ToString().ToUpper()} stream...");
        DisplayUtilities.ShowInfo("You may need to login first if not already authenticated");

        try
        {
            await _currentClient.StartStreamingAsync(
                data =>
                {
                    AnsiConsole.WriteLine();
                    DisplayUtilities.ShowInfo($"Stream update received at {DateTime.Now:HH:mm:ss}");
                    DisplayUtilities.ShowExchangeRateData(data, protocol.ToString().ToUpper());
                },
                _cts.Token
            );

            _isStreaming = true;
            DisplayUtilities.ShowSuccess($"{protocol.ToString().ToUpper()} stream started successfully");
            DisplayUtilities.ShowInfo("Use 'stream-stop' to stop streaming");
        }
        catch (Exception ex)
        {
            DisplayUtilities.ShowError($"Failed to start stream: {ex.Message}");
        }
    }

    private async Task HandleStopStreamingAsync()
    {
        if (!_isStreaming || _currentClient == null)
        {
            DisplayUtilities.ShowWarning("No active stream");
            return;
        }

        await _currentClient.StopStreamingAsync();
        _isStreaming = false;
        DisplayUtilities.ShowSuccess("Stream stopped");
    }

    private async Task HandleCompareAsync(string[] args)
    {
        var mode = args.Length > 0 ? args[0].ToLowerInvariant() : "latest";

        if (mode != "latest" && mode != "historical")
        {
            DisplayUtilities.ShowError("Invalid mode. Use: compare latest or compare historical");
            return;
        }

        DisplayUtilities.ShowInfo($"Comparing all APIs ({mode} rates)...");

        var results = new Dictionary<string, ApiCallMetrics>();

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var restTask = ctx.AddTask("[cyan1]REST[/]");
                var soapTask = ctx.AddTask("[yellow]SOAP[/]");
                var grpcTask = ctx.AddTask("[green]gRPC[/]");

                var clients = _factory.CreateAllClients();

                // Test REST
                var restClient = clients[ApiProtocol.Rest];
                var (_, restMetrics) = mode == "latest"
                    ? await restClient.GetLatestRatesAsync()
                    : await restClient.GetHistoricalRatesAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                results["REST"] = restMetrics;
                restTask.Value = 100;

                // Test SOAP
                var soapClient = clients[ApiProtocol.Soap];
                var (_, soapMetrics) = mode == "latest"
                    ? await soapClient.GetLatestRatesAsync()
                    : await soapClient.GetHistoricalRatesAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                results["SOAP"] = soapMetrics;
                soapTask.Value = 100;

                // Test gRPC
                var grpcClient = clients[ApiProtocol.Grpc];
                var (_, grpcMetrics) = mode == "latest"
                    ? await grpcClient.GetLatestRatesAsync()
                    : await grpcClient.GetHistoricalRatesAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                results["gRPC"] = grpcMetrics;
                grpcTask.Value = 100;
            });

        DisplayUtilities.ShowComparisonMetrics(results);
    }

    private async Task HandleSoloAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: solo <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        // Stop any active streaming
        if (_isStreaming)
        {
            await HandleStopStreamingAsync();
        }

        _currentClient = _factory.CreateClient(protocol);
        _currentProtocol = protocol;

        DisplayUtilities.ShowSuccess($"Solo mode activated for {protocol.ToString().ToUpper()}");
        DisplayUtilities.ShowInfo("You can now use login, latest, historical, and stream commands");
    }

    private void HandleStatus()
    {
        if (_currentClient == null || _currentProtocol == null)
        {
            DisplayUtilities.ShowWarning("No protocol selected. Use 'solo <protocol>' to select one");
            return;
        }

        DisplayUtilities.ShowStatus(
            _currentProtocol.Value.ToString().ToUpper(),
            _currentClient.IsAuthenticated,
            _isStreaming
        );
    }

    private async Task HandleGetCurrentAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: current <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching current rates from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetCurrentRatesAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    DisplayUtilities.ShowExchangeRateData(data, protocol.ToString().ToUpper());
                }
            });
    }

    private async Task HandleConvertAsync(string[] args)
    {
        if (args.Length < 4)
        {
            DisplayUtilities.ShowError("Usage: convert <protocol> <from> <to> <amount>");
            DisplayUtilities.ShowInfo("Example: convert rest EUR USD 100");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var from = args[1].ToUpper();
        var to = args[2].ToUpper();

        if (!decimal.TryParse(args[3], out var amount))
        {
            DisplayUtilities.ShowError("Invalid amount. Please provide a valid number");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Converting {amount} {from} to {to} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.ConvertCurrencyAsync(from, to, amount);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Currency Conversion[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("From", $"{data.Amount} {data.FromCurrency}");
                    table.AddRow("To", $"[green]{data.ConvertedAmount:F4} {data.ToCurrency}[/]");
                    table.AddRow("Rate", data.Rate.ToString("F6"));
                    table.AddRow("Valid Date", data.ValidDate.ToString("yyyy-MM-dd"));

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetCurrenciesAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: currencies <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching currencies from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetCurrenciesAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Currencies.Any())
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Code[/]")
                        .AddColumn("[bold]Name[/]")
                        .AddColumn("[bold]Symbol[/]")
                        .AddColumn("[bold]Decimals[/]")
                        .AddColumn("[bold]Active[/]");

                    foreach (var currency in data.Currencies)
                    {
                        table.AddRow(
                            currency.Code,
                            currency.Name,
                            currency.Symbol ?? "-",
                            currency.DecimalPlaces.ToString(),
                            currency.IsActive ? "[green]Yes[/]" : "[red]No[/]"
                        );
                    }

                    AnsiConsole.Write(table);
                    DisplayUtilities.ShowSuccess($"Total currencies: {data.Currencies.Count}");
                }
            });
    }

    private async Task HandleGetCurrencyAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: currency <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: currency rest EUR");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching currency {code} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetCurrencyAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Code", data.Code);
                    table.AddRow("Name", data.Name);
                    table.AddRow("Symbol", data.Symbol ?? "-");
                    table.AddRow("Decimal Places", data.DecimalPlaces.ToString());
                    table.AddRow("Active", data.IsActive ? "[green]Yes[/]" : "[red]No[/]");

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetProvidersAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: providers <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching providers from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetProvidersAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Providers.Any())
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Code[/]")
                        .AddColumn("[bold]Name[/]")
                        .AddColumn("[bold]URL[/]")
                        .AddColumn("[bold]Active[/]");

                    foreach (var provider in data.Providers)
                    {
                        table.AddRow(
                            provider.Code,
                            provider.Name,
                            provider.BaseUrl,
                            provider.IsActive ? "[green]Yes[/]" : "[red]No[/]"
                        );
                    }

                    AnsiConsole.Write(table);
                    DisplayUtilities.ShowSuccess($"Total providers: {data.Providers.Count}");
                }
            });
    }

    private async Task HandleGetProviderAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: provider <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: provider rest ECB");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching provider {code} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetProviderAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Code", data.Code);
                    table.AddRow("Name", data.Name);
                    table.AddRow("Base URL", data.BaseUrl);
                    table.AddRow("Active", data.IsActive ? "[green]Yes[/]" : "[red]No[/]");

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetProviderHealthAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: provider-health <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: provider-health rest ECB");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Checking health of provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetProviderHealthAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.ProviderCode))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Provider", $"{data.ProviderName} ({data.ProviderCode})");
                    table.AddRow("Status", data.IsHealthy ? "[green]Healthy[/]" : "[red]Unhealthy[/]");
                    table.AddRow("Consecutive Failures", data.ConsecutiveFailures.ToString());
                    table.AddRow("Last Success", data.LastSuccessfulFetch?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never");
                    table.AddRow("Last Failure", data.LastFailedFetch?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never");
                    if (!string.IsNullOrEmpty(data.LastError))
                    {
                        table.AddRow("Last Error", $"[red]{data.LastError}[/]");
                    }

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetProviderStatsAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: provider-stats <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: provider-stats rest ECB");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching statistics for provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetProviderStatisticsAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.ProviderCode))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Metric[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Provider Code", data.ProviderCode);
                    table.AddRow("Total Fetches", data.TotalFetches.ToString());
                    table.AddRow("Successful Fetches", $"[green]{data.SuccessfulFetches}[/]");
                    table.AddRow("Success Rate", $"{data.SuccessRate:P2}");
                    table.AddRow("Total Rates Provided", data.TotalRatesProvided.ToString());

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetUsersAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: users <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching users from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetUsersAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Users.Any())
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]ID[/]")
                        .AddColumn("[bold]Email[/]")
                        .AddColumn("[bold]Role[/]")
                        .AddColumn("[bold]Active[/]");

                    foreach (var user in data.Users)
                    {
                        table.AddRow(
                            user.Id.ToString(),
                            user.Email,
                            user.Role,
                            user.IsActive ? "[green]Yes[/]" : "[red]No[/]"
                        );
                    }

                    AnsiConsole.Write(table);
                    DisplayUtilities.ShowSuccess($"Total users: {data.Users.Count}");
                }
            });
    }

    private async Task HandleGetUserAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: user <protocol> <id>");
            DisplayUtilities.ShowInfo("Example: user rest 1");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var userId))
        {
            DisplayUtilities.ShowError("Invalid user ID. Please provide a valid number");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching user {userId} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetUserAsync(userId);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Id > 0)
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("ID", data.Id.ToString());
                    table.AddRow("Email", data.Email);
                    table.AddRow("Role", data.Role);
                    table.AddRow("Active", data.IsActive ? "[green]Yes[/]" : "[red]No[/]");
                    table.AddRow("Created", data.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleTestAllAsync(string[] args)
    {
        var protocol = args.Length > 0 && TryParseProtocol(args[0], out var p) ? p : (ApiProtocol?)null;

        if (protocol == null)
        {
            DisplayUtilities.ShowError("Usage: test-all <rest|soap|grpc>");
            DisplayUtilities.ShowInfo("This will test all endpoints for the specified protocol");
            return;
        }

        DisplayUtilities.ShowInfo($"Starting comprehensive test of all {protocol.Value.ToString().ToUpper()} endpoints...");

        var client = _factory.CreateClient(protocol.Value);
        var results = new Dictionary<string, (bool Success, long ResponseTimeMs, string Error)>();

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var tasks = new[]
                {
                    ("Current Rates", ctx.AddTask("[cyan1]Current Rates[/]")),
                    ("Latest Rates", ctx.AddTask("[cyan1]Latest Rates[/]")),
                    ("Historical Rates", ctx.AddTask("[cyan1]Historical Rates[/]")),
                    ("Convert Currency", ctx.AddTask("[cyan1]Convert Currency[/]")),
                    ("Get Currencies", ctx.AddTask("[cyan1]Get Currencies[/]")),
                    ("Get Currency", ctx.AddTask("[cyan1]Get Currency[/]")),
                    ("Get Providers", ctx.AddTask("[cyan1]Get Providers[/]")),
                    ("Get Provider", ctx.AddTask("[cyan1]Get Provider[/]")),
                    ("Provider Health", ctx.AddTask("[cyan1]Provider Health[/]")),
                    ("Provider Stats", ctx.AddTask("[cyan1]Provider Stats[/]")),
                    ("Get Users", ctx.AddTask("[cyan1]Get Users[/]")),
                    ("Get User", ctx.AddTask("[cyan1]Get User[/]"))
                };

                // Test Current Rates
                var (_, currentMetrics) = await client.GetCurrentRatesAsync();
                results["Current Rates"] = (currentMetrics.Success, currentMetrics.ResponseTimeMs, currentMetrics.ErrorMessage ?? "");
                tasks[0].Item2.Value = 100;

                // Test Latest Rates
                var (_, latestMetrics) = await client.GetLatestRatesAsync();
                results["Latest Rates"] = (latestMetrics.Success, latestMetrics.ResponseTimeMs, latestMetrics.ErrorMessage ?? "");
                tasks[1].Item2.Value = 100;

                // Test Historical Rates
                var (_, historicalMetrics) = await client.GetHistoricalRatesAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                results["Historical Rates"] = (historicalMetrics.Success, historicalMetrics.ResponseTimeMs, historicalMetrics.ErrorMessage ?? "");
                tasks[2].Item2.Value = 100;

                // Test Convert Currency
                var (_, convertMetrics) = await client.ConvertCurrencyAsync("EUR", "USD", 100);
                results["Convert Currency"] = (convertMetrics.Success, convertMetrics.ResponseTimeMs, convertMetrics.ErrorMessage ?? "");
                tasks[3].Item2.Value = 100;

                // Test Get Currencies
                var (_, currenciesMetrics) = await client.GetCurrenciesAsync();
                results["Get Currencies"] = (currenciesMetrics.Success, currenciesMetrics.ResponseTimeMs, currenciesMetrics.ErrorMessage ?? "");
                tasks[4].Item2.Value = 100;

                // Test Get Currency
                var (_, currencyMetrics) = await client.GetCurrencyAsync("EUR");
                results["Get Currency"] = (currencyMetrics.Success, currencyMetrics.ResponseTimeMs, currencyMetrics.ErrorMessage ?? "");
                tasks[5].Item2.Value = 100;

                // Test Get Providers
                var (_, providersMetrics) = await client.GetProvidersAsync();
                results["Get Providers"] = (providersMetrics.Success, providersMetrics.ResponseTimeMs, providersMetrics.ErrorMessage ?? "");
                tasks[6].Item2.Value = 100;

                // Test Get Provider
                var (_, providerMetrics) = await client.GetProviderAsync("ECB");
                results["Get Provider"] = (providerMetrics.Success, providerMetrics.ResponseTimeMs, providerMetrics.ErrorMessage ?? "");
                tasks[7].Item2.Value = 100;

                // Test Provider Health
                var (_, healthMetrics) = await client.GetProviderHealthAsync("ECB");
                results["Provider Health"] = (healthMetrics.Success, healthMetrics.ResponseTimeMs, healthMetrics.ErrorMessage ?? "");
                tasks[8].Item2.Value = 100;

                // Test Provider Stats
                var (_, statsMetrics) = await client.GetProviderStatisticsAsync("ECB");
                results["Provider Stats"] = (statsMetrics.Success, statsMetrics.ResponseTimeMs, statsMetrics.ErrorMessage ?? "");
                tasks[9].Item2.Value = 100;

                // Test Get Users
                var (_, usersMetrics) = await client.GetUsersAsync();
                results["Get Users"] = (usersMetrics.Success, usersMetrics.ResponseTimeMs, usersMetrics.ErrorMessage ?? "");
                tasks[10].Item2.Value = 100;

                // Test Get User
                var (_, userMetrics) = await client.GetUserAsync(1);
                results["Get User"] = (userMetrics.Success, userMetrics.ResponseTimeMs, userMetrics.ErrorMessage ?? "");
                tasks[11].Item2.Value = 100;
            });

        // Display comprehensive results
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title($"[bold]{protocol.Value.ToString().ToUpper()} - Complete Endpoint Test Results[/]")
            .AddColumn("[bold]Endpoint[/]")
            .AddColumn("[bold]Status[/]")
            .AddColumn("[bold]Response Time[/]")
            .AddColumn("[bold]Error[/]");

        foreach (var result in results)
        {
            table.AddRow(
                result.Key,
                result.Value.Success ? "[green]✓ Pass[/]" : "[red]✗ Fail[/]",
                $"{result.Value.ResponseTimeMs}ms",
                string.IsNullOrEmpty(result.Value.Error) ? "-" : $"[red]{result.Value.Error}[/]"
            );
        }

        AnsiConsole.Write(table);

        var totalTests = results.Count;
        var passedTests = results.Count(r => r.Value.Success);
        var failedTests = totalTests - passedTests;

        DisplayUtilities.ShowInfo($"Tests completed: {passedTests}/{totalTests} passed, {failedTests} failed");
    }

    private async Task CleanupAndExitAsync()
    {
        DisplayUtilities.ShowInfo("Shutting down...");

        if (_isStreaming && _currentClient != null)
        {
            await _currentClient.StopStreamingAsync();
        }

        _cts.Cancel();
        DisplayUtilities.ShowSuccess("Goodbye!");
    }

    private static bool TryParseProtocol(string input, out ApiProtocol protocol)
    {
        return input.ToLowerInvariant() switch
        {
            "rest" => SetProtocol(out protocol, ApiProtocol.Rest),
            "soap" => SetProtocol(out protocol, ApiProtocol.Soap),
            "grpc" => SetProtocol(out protocol, ApiProtocol.Grpc),
            _ => SetProtocol(out protocol, default)
        };

        static bool SetProtocol(out ApiProtocol p, ApiProtocol value)
        {
            p = value;
            return value != default;
        }
    }
}
