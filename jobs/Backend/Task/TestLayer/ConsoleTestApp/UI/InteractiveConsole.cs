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
    private readonly SmartConsoleInput _inputHandler;

    private IApiClient? _currentClient;
    private ApiProtocol? _currentProtocol;
    private bool _isStreaming = false;

    public InteractiveConsole(AppSettings settings)
    {
        _factory = new ApiClientFactory(settings.ApiEndpoints);
        _credentials = settings.TestCredentials;

        var autocomplete = new AutoCompleteEngine(CommandParser.GetAutoCompleteOptions());
        _inputHandler = new SmartConsoleInput(autocomplete);
    }

    public async Task RunAsync()
    {
        DisplayUtilities.ShowWelcome();

        while (!_cts.Token.IsCancellationRequested)
        {
            try
            {
                var input = _inputHandler.ReadLine();

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

            case CommandType.LoginAll:
                await HandleLoginAllAsync(command.Arguments);
                break;

            case CommandType.Logout:
                await HandleLogoutAsync(command.Arguments);
                break;

            case CommandType.LogoutAll:
                await HandleLogoutAllAsync();
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

            case CommandType.ExitSolo:
                HandleExitSolo();
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

            case CommandType.RescheduleProvider:
                await HandleRescheduleProviderAsync(command.Arguments);
                break;

            case CommandType.GetUsers:
                await HandleGetUsersAsync(command.Arguments);
                break;

            case CommandType.GetUser:
                await HandleGetUserAsync(command.Arguments);
                break;

            case CommandType.Test:
                await HandleTestAsync(command.Arguments);
                break;

            case CommandType.TestAll:
                await HandleTestAllAsync(command.Arguments);
                break;

            case CommandType.IsApiAvailable:
                await HandleIsApiAvailableAsync(command.Arguments);
                break;

            case CommandType.GetCurrentGrouped:
                await HandleGetCurrentGroupedAsync(command.Arguments);
                break;

            case CommandType.GetLatestRate:
                await HandleGetLatestRateAsync(command.Arguments);
                break;

            case CommandType.GetCurrencyById:
                await HandleGetCurrencyByIdAsync(command.Arguments);
                break;

            case CommandType.CreateCurrency:
                await HandleCreateCurrencyAsync(command.Arguments);
                break;

            case CommandType.DeleteCurrency:
                await HandleDeleteCurrencyAsync(command.Arguments);
                break;

            case CommandType.GetProviderById:
                await HandleGetProviderByIdAsync(command.Arguments);
                break;

            case CommandType.GetProviderConfiguration:
                await HandleGetProviderConfigurationAsync(command.Arguments);
                break;

            case CommandType.ActivateProvider:
                await HandleActivateProviderAsync(command.Arguments);
                break;

            case CommandType.DeactivateProvider:
                await HandleDeactivateProviderAsync(command.Arguments);
                break;

            case CommandType.ResetProviderHealth:
                await HandleResetProviderHealthAsync(command.Arguments);
                break;

            case CommandType.TriggerManualFetch:
                await HandleTriggerManualFetchAsync(command.Arguments);
                break;

            case CommandType.CreateProvider:
                await HandleCreateProviderAsync(command.Arguments);
                break;

            case CommandType.UpdateProviderConfiguration:
                await HandleUpdateProviderConfigurationAsync(command.Arguments);
                break;

            case CommandType.DeleteProvider:
                await HandleDeleteProviderAsync(command.Arguments);
                break;

            case CommandType.GetUserByEmail:
                await HandleGetUserByEmailAsync(command.Arguments);
                break;

            case CommandType.GetUsersByRole:
                await HandleGetUsersByRoleAsync(command.Arguments);
                break;

            case CommandType.CheckEmailExists:
                await HandleCheckEmailExistsAsync(command.Arguments);
                break;

            case CommandType.CreateUser:
                await HandleCreateUserAsync(command.Arguments);
                break;

            case CommandType.UpdateUser:
                await HandleUpdateUserAsync(command.Arguments);
                break;

            case CommandType.ChangePassword:
                await HandleChangePasswordAsync(command.Arguments);
                break;

            case CommandType.ChangeUserRole:
                await HandleChangeUserRoleAsync(command.Arguments);
                break;

            case CommandType.DeleteUser:
                await HandleDeleteUserAsync(command.Arguments);
                break;

            case CommandType.GetSystemHealth:
                await HandleGetSystemHealthAsync(command.Arguments);
                break;

            case CommandType.GetRecentErrors:
                await HandleGetRecentErrorsAsync(command.Arguments);
                break;

            case CommandType.GetFetchActivity:
                await HandleGetFetchActivityAsync(command.Arguments);
                break;

            case CommandType.Invalid:
                DisplayUtilities.ShowError($"Unknown command: {string.Join(" ", command.Arguments)}");
                DisplayUtilities.ShowInfo("Type 'help' for available commands");
                break;
        }
    }

    private async Task HandleLoginAsync(string[] args)
    {
        // In solo mode, use current client; otherwise require protocol argument
        IApiClient client;
        ApiProtocol protocol;
        string email, password;

        if (_currentClient != null && _currentProtocol.HasValue)
        {
            // Solo mode: protocol is already set, args are [email] [password]
            client = _currentClient;
            protocol = _currentProtocol.Value;

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
        }
        else
        {
            // Normal mode: args are <protocol> [email] [password]
            if (args.Length == 0)
            {
                DisplayUtilities.ShowError("Usage: login <protocol> [email] [password]");
                DisplayUtilities.ShowInfo("Example: login rest admin@example.com simple");
                DisplayUtilities.ShowInfo("Example: login grpc (uses default admin credentials)");
                return;
            }

            if (!TryParseProtocol(args[0], out protocol))
            {
                DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
                return;
            }

            if (args.Length >= 3)
            {
                email = args[1];
                password = args[2];
            }
            else
            {
                // Use default admin credentials
                email = _credentials.Admin.Email;
                password = _credentials.Admin.Password;
                DisplayUtilities.ShowInfo($"Using default admin credentials: {email}");
            }

            client = _factory.CreateClient(protocol);
        }

        await AnsiConsole.Status()
            .StartAsync($"Logging in to {protocol.ToString().ToUpper()}...", async ctx =>
            {
                var result = await client.LoginAsync(email, password);

                if (result.Success)
                {
                    DisplayUtilities.ShowSuccess($"[{protocol.ToString().ToUpper()}] Logged in as {result.Email} ({result.Role})");
                    DisplayUtilities.ShowInfo($"Token expires at: {result.ExpiresAt:yyyy-MM-dd HH:mm:ss}");
                }
                else
                {
                    DisplayUtilities.ShowError($"[{protocol.ToString().ToUpper()}] Login failed: {result.ErrorMessage}");
                }
            });
    }

    private async Task HandleLoginAllAsync(string[] args)
    {
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

        var protocols = new[] { ApiProtocol.Rest, ApiProtocol.Soap, ApiProtocol.Grpc };
        var results = new Dictionary<string, (bool Success, string Message)>();

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var restTask = ctx.AddTask("[cyan1]REST[/]");
                var soapTask = ctx.AddTask("[yellow]SOAP[/]");
                var grpcTask = ctx.AddTask("[green]gRPC[/]");

                // Login to REST
                var restClient = _factory.CreateClient(ApiProtocol.Rest);
                var restResult = await restClient.LoginAsync(email, password);
                results["REST"] = (restResult.Success, restResult.Success ? $"{restResult.Email} ({restResult.Role})" : restResult.ErrorMessage ?? "Login failed");
                restTask.Value = 100;

                // Login to SOAP
                var soapClient = _factory.CreateClient(ApiProtocol.Soap);
                var soapResult = await soapClient.LoginAsync(email, password);
                results["SOAP"] = (soapResult.Success, soapResult.Success ? $"{soapResult.Email} ({soapResult.Role})" : soapResult.ErrorMessage ?? "Login failed");
                soapTask.Value = 100;

                // Login to gRPC
                var grpcClient = _factory.CreateClient(ApiProtocol.Grpc);
                var grpcResult = await grpcClient.LoginAsync(email, password);
                results["gRPC"] = (grpcResult.Success, grpcResult.Success ? $"{grpcResult.Email} ({grpcResult.Role})" : grpcResult.ErrorMessage ?? "Login failed");
                grpcTask.Value = 100;
            });

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Login Results[/]")
            .AddColumn("[bold]Protocol[/]")
            .AddColumn("[bold]Status[/]")
            .AddColumn("[bold]Details[/]");

        foreach (var result in results)
        {
            table.AddRow(
                result.Key,
                result.Value.Success ? "[green]✓ Success[/]" : "[red]✗ Failed[/]",
                result.Value.Message
            );
        }

        AnsiConsole.Write(table);

        var successCount = results.Count(r => r.Value.Success);
        if (successCount == 3)
        {
            DisplayUtilities.ShowSuccess("Successfully logged in to all protocols");
        }
        else
        {
            DisplayUtilities.ShowWarning($"Logged in to {successCount}/3 protocols");
        }
    }

    private async Task HandleLogoutAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Usage: logout <protocol>");
            DisplayUtilities.ShowInfo("Example: logout rest");
            DisplayUtilities.ShowInfo("Use 'logout-all' to logout from all protocols");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);
        await client.LogoutAsync();
        DisplayUtilities.ShowSuccess($"[{protocol.ToString().ToUpper()}] Logged out successfully");
    }

    private async Task HandleLogoutAllAsync()
    {
        var protocols = new[] { ApiProtocol.Rest, ApiProtocol.Soap, ApiProtocol.Grpc };

        foreach (var protocol in protocols)
        {
            var client = _factory.CreateClient(protocol);
            await client.LogoutAsync();
        }

        DisplayUtilities.ShowSuccess("Logged out from all protocols");
    }

    private async Task HandleGetLatestAsync(string[] args)
    {
        // In solo mode, use current client; otherwise require protocol argument
        IApiClient client;
        ApiProtocol protocol;

        if (_currentClient != null && _currentProtocol.HasValue)
        {
            client = _currentClient;
            protocol = _currentProtocol.Value;
        }
        else
        {
            if (args.Length == 0)
            {
                DisplayUtilities.ShowError("Please specify protocol: latest <rest|soap|grpc>");
                return;
            }

            if (!TryParseProtocol(args[0], out protocol))
            {
                DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
                return;
            }

            client = _factory.CreateClient(protocol);
        }

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
        // In solo mode, use current client; otherwise require protocol argument
        IApiClient client;
        ApiProtocol protocol;

        if (_currentClient != null && _currentProtocol.HasValue)
        {
            client = _currentClient;
            protocol = _currentProtocol.Value;
        }
        else
        {
            if (args.Length == 0)
            {
                DisplayUtilities.ShowError("Please specify protocol: historical <rest|soap|grpc>");
                return;
            }

            if (!TryParseProtocol(args[0], out protocol))
            {
                DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
                return;
            }

            client = _factory.CreateClient(protocol);
        }
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
        ApiProtocol protocol;

        // In solo mode, use current protocol; otherwise require protocol argument
        if (_currentClient != null && _currentProtocol.HasValue)
        {
            protocol = _currentProtocol.Value;
        }
        else
        {
            if (args.Length == 0)
            {
                DisplayUtilities.ShowError("Please specify protocol: stream <rest|soap|grpc>");
                return;
            }

            if (!TryParseProtocol(args[0], out protocol))
            {
                DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
                return;
            }
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
                    DisplayUtilities.ShowInfo($"Stream update received at {DateTime.UtcNow:HH:mm:ss} UTC");
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

    private void HandleExitSolo()
    {
        if (_currentClient == null && _currentProtocol == null)
        {
            DisplayUtilities.ShowWarning("You are not in solo mode");
            return;
        }

        _currentClient = null;
        _currentProtocol = null;

        DisplayUtilities.ShowSuccess("Exited solo mode");
        DisplayUtilities.ShowInfo("You can now use commands with explicit protocol arguments");
    }

    private void HandleStatus()
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]System Status[/]")
            .AddColumn("[bold]Protocol[/]")
            .AddColumn("[bold]Authenticated[/]")
            .AddColumn("[bold]Streaming[/]");

        var protocols = new[] { ApiProtocol.Rest, ApiProtocol.Soap, ApiProtocol.Grpc };

        foreach (var protocol in protocols)
        {
            var client = _factory.CreateClient(protocol);
            var isStreaming = _isStreaming && _currentProtocol == protocol;

            table.AddRow(
                protocol.ToString().ToUpper(),
                client.IsAuthenticated ? "[green]✓ Yes[/]" : "[red]✗ No[/]",
                isStreaming ? "[green]✓ Active[/]" : "[grey]○ Inactive[/]"
            );
        }

        AnsiConsole.Write(table);
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
                var (data, metrics) = await client.GetCurrencyByCodeAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Code", Markup.Escape(data.Code ?? "-"));
                    table.AddRow("Name", Markup.Escape(data.Name ?? data.Code ?? "-"));
                    table.AddRow("Symbol", Markup.Escape(data.Symbol ?? "-"));
                    table.AddRow("Decimal Places", Markup.Escape(data.DecimalPlaces.ToString()));
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
                var (data, metrics) = await client.GetProviderByCodeAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Code", Markup.Escape(data.Code ?? ""));
                    table.AddRow("Name", Markup.Escape(data.Name ?? ""));
                    table.AddRow("Base URL", Markup.Escape(data.BaseUrl ?? "N/A"));
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

    private async Task HandleRescheduleProviderAsync(string[] args)
    {
        if (args.Length < 4)
        {
            DisplayUtilities.ShowError("Usage: reschedule-provider <protocol> <code> <time> <timezone>");
            DisplayUtilities.ShowInfo("Example: reschedule-provider rest ECB 14:30 UTC");
            DisplayUtilities.ShowInfo("Example: reschedule-provider grpc CNB 16:00 CET");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();
        var updateTime = args[2];
        var timeZone = args[3];
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Rescheduling provider {code} to {updateTime} ({timeZone}) via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.RescheduleProviderAsync(code, updateTime, timeZone);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
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

    private async Task HandleTestAsync(string[] args)
    {
        var protocol = args.Length > 0 && TryParseProtocol(args[0], out var p) ? p : (ApiProtocol?)null;

        if (protocol == null)
        {
            DisplayUtilities.ShowError("Usage: test <rest|soap|grpc>");
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
                var (_, currencyMetrics) = await client.GetCurrencyByCodeAsync("EUR");
                results["Get Currency"] = (currencyMetrics.Success, currencyMetrics.ResponseTimeMs, currencyMetrics.ErrorMessage ?? "");
                tasks[5].Item2.Value = 100;

                // Test Get Providers
                var (_, providersMetrics) = await client.GetProvidersAsync();
                results["Get Providers"] = (providersMetrics.Success, providersMetrics.ResponseTimeMs, providersMetrics.ErrorMessage ?? "");
                tasks[6].Item2.Value = 100;

                // Test Get Provider
                var (_, providerMetrics) = await client.GetProviderByCodeAsync("ECB");
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

    private async Task HandleTestAllAsync(string[] args)
    {
        DisplayUtilities.ShowInfo("Starting comprehensive test of all protocols (REST, SOAP, gRPC)...");

        var protocols = new[] { ApiProtocol.Rest, ApiProtocol.Soap, ApiProtocol.Grpc };
        var allResults = new Dictionary<ApiProtocol, Dictionary<string, (bool Success, long ResponseTimeMs, string Error)>>();

        foreach (var protocol in protocols)
        {
            DisplayUtilities.ShowInfo($"\nTesting {protocol.ToString().ToUpper()} endpoints...");

            var client = _factory.CreateClient(protocol);
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
                    var (_, currencyMetrics) = await client.GetCurrencyByCodeAsync("EUR");
                    results["Get Currency"] = (currencyMetrics.Success, currencyMetrics.ResponseTimeMs, currencyMetrics.ErrorMessage ?? "");
                    tasks[5].Item2.Value = 100;

                    // Test Get Providers
                    var (_, providersMetrics) = await client.GetProvidersAsync();
                    results["Get Providers"] = (providersMetrics.Success, providersMetrics.ResponseTimeMs, providersMetrics.ErrorMessage ?? "");
                    tasks[6].Item2.Value = 100;

                    // Test Get Provider
                    var (_, providerMetrics) = await client.GetProviderByCodeAsync("ECB");
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

            allResults[protocol] = results;
        }

        // Display comprehensive results for all protocols
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Complete Endpoint Test Results - All Protocols[/]")
            .AddColumn("[bold]Endpoint[/]")
            .AddColumn("[bold]REST[/]")
            .AddColumn("[bold]SOAP[/]")
            .AddColumn("[bold]gRPC[/]");

        // Get all unique endpoint names
        var endpointNames = allResults[ApiProtocol.Rest].Keys;

        foreach (var endpoint in endpointNames)
        {
            var restResult = allResults[ApiProtocol.Rest][endpoint];
            var soapResult = allResults[ApiProtocol.Soap][endpoint];
            var grpcResult = allResults[ApiProtocol.Grpc][endpoint];

            table.AddRow(
                endpoint,
                FormatResult(restResult),
                FormatResult(soapResult),
                FormatResult(grpcResult)
            );
        }

        AnsiConsole.Write(table);

        // Display summary
        DisplayUtilities.ShowInfo("\n[bold]Summary:[/]");
        foreach (var protocol in protocols)
        {
            var results = allResults[protocol];
            var totalTests = results.Count;
            var passedTests = results.Count(r => r.Value.Success);
            var failedTests = totalTests - passedTests;
            DisplayUtilities.ShowInfo($"{protocol.ToString().ToUpper()}: {passedTests}/{totalTests} passed, {failedTests} failed");
        }
    }

    private string FormatResult((bool Success, long ResponseTimeMs, string Error) result)
    {
        if (result.Success)
        {
            return $"[green]✓ {result.ResponseTimeMs}ms[/]";
        }
        else
        {
            var errorShort = string.IsNullOrEmpty(result.Error) ? "?" :
                             result.Error.Contains("Unauthorized") ? "401" :
                             result.Error.Contains("NotFound") ? "404" :
                             result.Error.Contains("BadRequest") ? "400" : "Err";
            return $"[red]✗ {errorShort}[/]";
        }
    }

    private async Task HandleIsApiAvailableAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: check-api <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Checking if {protocol} API is available...", async ctx =>
            {
                var isAvailable = await client.IsApiAvailableAsync();

                if (isAvailable)
                {
                    DisplayUtilities.ShowSuccess($"{protocol.ToString().ToUpper()} API is available");
                }
                else
                {
                    DisplayUtilities.ShowError($"{protocol.ToString().ToUpper()} API is not available");
                }
            });
    }

    private async Task HandleGetCurrentGroupedAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: current-grouped <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching current rates (grouped) from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetCurrentRatesGroupedAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    DisplayUtilities.ShowExchangeRateData(data, protocol.ToString().ToUpper());
                }
            });
    }

    private async Task HandleGetLatestRateAsync(string[] args)
    {
        if (args.Length < 3)
        {
            DisplayUtilities.ShowError("Usage: latest-rate <protocol> <source> <target> [providerId]");
            DisplayUtilities.ShowInfo("Example: latest-rate rest EUR USD");
            DisplayUtilities.ShowInfo("Example: latest-rate grpc EUR USD 1");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var source = args[1].ToUpper();
        var target = args[2].ToUpper();
        int? providerId = null;

        if (args.Length >= 4 && int.TryParse(args[3], out var pid))
        {
            providerId = pid;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching latest rate {source}/{target} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetLatestRateAsync(source, target, providerId);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    DisplayUtilities.ShowExchangeRateData(data, protocol.ToString().ToUpper());
                }
            });
    }

    private async Task HandleGetCurrencyByIdAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: currency-id <protocol> <id>");
            DisplayUtilities.ShowInfo("Example: currency-id rest 1");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var id))
        {
            DisplayUtilities.ShowError("Invalid currency ID. Please provide a valid number");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching currency {id} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetCurrencyAsync(id);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("ID", data.Id.ToString());
                    table.AddRow("Code", Markup.Escape(data.Code ?? ""));
                    table.AddRow("Name", Markup.Escape(data.Name ?? ""));
                    table.AddRow("Symbol", Markup.Escape(data.Symbol ?? "-"));
                    table.AddRow("Decimal Places", data.DecimalPlaces.ToString());
                    table.AddRow("Active", data.IsActive ? "[green]Yes[/]" : "[red]No[/]");

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleCreateCurrencyAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: create-currency <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: create-currency rest GBP");
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
            .StartAsync($"Creating currency {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.CreateCurrencyAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleDeleteCurrencyAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: delete-currency <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: delete-currency rest GBP");
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
            .StartAsync($"Deleting currency {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.DeleteCurrencyAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleGetProviderByIdAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: provider-id <protocol> <id>");
            DisplayUtilities.ShowInfo("Example: provider-id rest 1");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var id))
        {
            DisplayUtilities.ShowError("Invalid provider ID. Please provide a valid number");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching provider {id} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetProviderAsync(id);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("ID", data.Id.ToString());
                    table.AddRow("Code", Markup.Escape(data.Code ?? ""));
                    table.AddRow("Name", Markup.Escape(data.Name ?? ""));
                    table.AddRow("Base URL", Markup.Escape(data.BaseUrl ?? "N/A"));
                    table.AddRow("Active", data.IsActive ? "[green]Yes[/]" : "[red]No[/]");

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetProviderConfigurationAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: provider-config <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: provider-config rest ECB");
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
            .StartAsync($"Fetching configuration for provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetProviderConfigurationAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && !string.IsNullOrEmpty(data.Code))
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Property[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Provider Code", Markup.Escape(data.Code ?? "-"));
                    table.AddRow("Name", Markup.Escape(data.Name ?? "-"));
                    table.AddRow("URL", Markup.Escape(data.Url ?? "-"));
                    table.AddRow("Description", Markup.Escape(data.Description ?? "-"));
                    table.AddRow("Base Currency Code", Markup.Escape(data.BaseCurrencyCode ?? "-"));
                    table.AddRow("Requires Auth", data.RequiresAuthentication ? "[green]Yes[/]" : "[red]No[/]");
                    table.AddRow("Is Active", data.IsActive ? "[green]Yes[/]" : "[red]No[/]");

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleActivateProviderAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: activate-provider <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: activate-provider rest ECB");
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
            .StartAsync($"Activating provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.ActivateProviderAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleDeactivateProviderAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: deactivate-provider <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: deactivate-provider rest ECB");
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
            .StartAsync($"Deactivating provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.DeactivateProviderAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleResetProviderHealthAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: reset-provider-health <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: reset-provider-health rest ECB");
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
            .StartAsync($"Resetting health for provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.ResetProviderHealthAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleTriggerManualFetchAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: trigger-fetch <protocol> <code>");
            DisplayUtilities.ShowInfo("Example: trigger-fetch rest ECB");
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
            .StartAsync($"Triggering manual fetch for provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.TriggerManualFetchAsync(code);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleCreateProviderAsync(string[] args)
    {
        if (args.Length < 6)
        {
            DisplayUtilities.ShowError("Usage: create-provider <protocol> <name> <code> <url> <baseCurrencyId> <requiresAuth> [apiKeyRef]");
            DisplayUtilities.ShowInfo("Example: create-provider rest \"European Central Bank\" ECB https://api.ecb.eu 1 false");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var name = args[1];
        var code = args[2].ToUpper();
        var url = args[3];

        if (!int.TryParse(args[4], out var baseCurrencyId))
        {
            DisplayUtilities.ShowError("Invalid base currency ID. Please provide a valid number");
            return;
        }

        if (!bool.TryParse(args[5], out var requiresAuth))
        {
            DisplayUtilities.ShowError("Invalid requiresAuth value. Use: true or false");
            return;
        }

        string? apiKeyRef = args.Length >= 7 ? args[6] : null;

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Creating provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.CreateProviderAsync(name, code, url, baseCurrencyId, requiresAuth, apiKeyRef);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleUpdateProviderConfigurationAsync(string[] args)
    {
        if (args.Length < 5)
        {
            DisplayUtilities.ShowError("Usage: update-provider-config <protocol> <code> <name> <url> <requiresAuth> [apiKeyRef]");
            DisplayUtilities.ShowInfo("Example: update-provider-config rest ECB \"European Bank\" https://new-api.ecb.eu false");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();
        var name = args[2];
        var url = args[3];

        if (!bool.TryParse(args[4], out var requiresAuth))
        {
            DisplayUtilities.ShowError("Invalid requiresAuth value. Use: true or false");
            return;
        }

        string? apiKeyRef = args.Length >= 6 ? args[5] : null;

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Updating configuration for provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.UpdateProviderConfigurationAsync(code, name, url, requiresAuth, apiKeyRef);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleDeleteProviderAsync(string[] args)
    {
        if (args.Length < 3)
        {
            DisplayUtilities.ShowError("Usage: delete-provider <protocol> <code> <force>");
            DisplayUtilities.ShowInfo("Example: delete-provider rest ECB false");
            DisplayUtilities.ShowInfo("Example: delete-provider grpc CNB true");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var code = args[1].ToUpper();

        if (!bool.TryParse(args[2], out var force))
        {
            DisplayUtilities.ShowError("Invalid force value. Use: true or false");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Deleting provider {code} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.DeleteProviderAsync(code, force);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleGetUserByEmailAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: user-by-email <protocol> <email>");
            DisplayUtilities.ShowInfo("Example: user-by-email rest admin@example.com");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var email = args[1];
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching user by email {email} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetUserByEmailAsync(email);

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

    private async Task HandleGetUsersByRoleAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: users-by-role <protocol> <role>");
            DisplayUtilities.ShowInfo("Example: users-by-role rest Admin");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var role = args[1];
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching users with role {role} from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetUsersByRoleAsync(role);

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

    private async Task HandleCheckEmailExistsAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: check-email <protocol> <email>");
            DisplayUtilities.ShowInfo("Example: check-email rest user@example.com");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var email = args[1];
        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Checking if email {email} exists via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.CheckEmailExistsAsync(email);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] Email exists: {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]○[/] Email does not exist");
                }
            });
    }

    private async Task HandleCreateUserAsync(string[] args)
    {
        if (args.Length < 6)
        {
            DisplayUtilities.ShowError("Usage: create-user <protocol> <email> <password> <firstName> <lastName> <role>");
            DisplayUtilities.ShowInfo("Example: create-user rest john@example.com Pass123! John Doe User");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var email = args[1];
        var password = args[2];
        var firstName = args[3];
        var lastName = args[4];
        var role = args[5];

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Creating user {email} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.CreateUserAsync(email, password, firstName, lastName, role);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleUpdateUserAsync(string[] args)
    {
        if (args.Length < 4)
        {
            DisplayUtilities.ShowError("Usage: update-user <protocol> <id> <firstName> <lastName>");
            DisplayUtilities.ShowInfo("Example: update-user rest 1 John Doe");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var id))
        {
            DisplayUtilities.ShowError("Invalid user ID. Please provide a valid number");
            return;
        }

        var firstName = args[2];
        var lastName = args[3];

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Updating user {id} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.UpdateUserAsync(id, firstName, lastName);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleChangePasswordAsync(string[] args)
    {
        if (args.Length < 4)
        {
            DisplayUtilities.ShowError("Usage: change-password <protocol> <id> <currentPassword> <newPassword>");
            DisplayUtilities.ShowInfo("Example: change-password rest 1 OldPass123! NewPass456!");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var id))
        {
            DisplayUtilities.ShowError("Invalid user ID. Please provide a valid number");
            return;
        }

        var currentPassword = args[2];
        var newPassword = args[3];

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Changing password for user {id} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.ChangePasswordAsync(id, currentPassword, newPassword);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleChangeUserRoleAsync(string[] args)
    {
        if (args.Length < 3)
        {
            DisplayUtilities.ShowError("Usage: change-user-role <protocol> <id> <newRole>");
            DisplayUtilities.ShowInfo("Example: change-user-role rest 1 Admin");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var id))
        {
            DisplayUtilities.ShowError("Invalid user ID. Please provide a valid number");
            return;
        }

        var newRole = args[2];

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Changing role for user {id} to {newRole} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.ChangeUserRoleAsync(id, newRole);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleDeleteUserAsync(string[] args)
    {
        if (args.Length < 2)
        {
            DisplayUtilities.ShowError("Usage: delete-user <protocol> <id>");
            DisplayUtilities.ShowInfo("Example: delete-user rest 5");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        if (!int.TryParse(args[1], out var id))
        {
            DisplayUtilities.ShowError("Invalid user ID. Please provide a valid number");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Deleting user {id} via {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.DeleteUserAsync(id);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Success)
                {
                    AnsiConsole.MarkupLine($"[green]✓[/] {data.Message}");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]✗[/] {data.ErrorMessage}");
                }
            });
    }

    private async Task HandleGetSystemHealthAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Please specify protocol: system-health <rest|soap|grpc>");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching system health from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetSystemHealthAsync();

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success)
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Metric[/]")
                        .AddColumn("[bold]Value[/]");

                    table.AddRow("Status", data.Status ?? "Unknown");
                    table.AddRow("Total Providers", data.TotalProviders.ToString());
                    table.AddRow("Healthy Providers", data.HealthyProviders.ToString());
                    table.AddRow("Unhealthy Providers", data.UnhealthyProviders.ToString());
                    table.AddRow("Total Currencies", data.TotalCurrencies.ToString());
                    table.AddRow("Total Users", data.TotalUsers.ToString());
                    table.AddRow("Total Exchange Rates", data.TotalExchangeRates.ToString());
                    table.AddRow("Last Fetch Time", data.LastFetchTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never");
                    table.AddRow("System Uptime (hrs)", data.SystemUptime.ToString("F2"));

                    AnsiConsole.Write(table);
                }
            });
    }

    private async Task HandleGetRecentErrorsAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Usage: errors <protocol> [count] [severity]");
            DisplayUtilities.ShowInfo("Example: errors rest 10 Error");
            DisplayUtilities.ShowInfo("Example: errors grpc 20");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        int count = 20;
        if (args.Length >= 2 && int.TryParse(args[1], out var c))
        {
            count = c;
        }

        string? severity = args.Length >= 3 ? args[2] : null;

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching recent errors from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetRecentErrorsAsync(count, severity);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Errors.Any())
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Timestamp[/]")
                        .AddColumn("[bold]Severity[/]")
                        .AddColumn("[bold]Provider[/]")
                        .AddColumn("[bold]Message[/]");

                    foreach (var error in data.Errors)
                    {
                        var severityColor = error.Severity?.ToLower() == "error" ? "red" : "yellow";
                        table.AddRow(
                            Markup.Escape(error.OccurredAt.ToString("yyyy-MM-dd HH:mm:ss")),
                            $"[{severityColor}]{Markup.Escape(error.Severity ?? "Unknown")}[/]",
                            Markup.Escape(error.ProviderCode ?? "-"),
                            Markup.Escape(error.ErrorMessage ?? "-")
                        );
                    }

                    AnsiConsole.Write(table);
                    DisplayUtilities.ShowSuccess($"Total errors: {data.Errors.Count}");
                }
                else
                {
                    DisplayUtilities.ShowInfo("No errors found");
                }
            });
    }

    private async Task HandleGetFetchActivityAsync(string[] args)
    {
        if (args.Length == 0)
        {
            DisplayUtilities.ShowError("Usage: fetch-activity <protocol> [count] [providerId] [failedOnly]");
            DisplayUtilities.ShowInfo("Example: fetch-activity rest 10");
            DisplayUtilities.ShowInfo("Example: fetch-activity grpc 20 1 true");
            return;
        }

        if (!TryParseProtocol(args[0], out var protocol))
        {
            DisplayUtilities.ShowError("Invalid protocol. Use: rest, soap, or grpc");
            return;
        }

        int count = 20;
        if (args.Length >= 2 && int.TryParse(args[1], out var c))
        {
            count = c;
        }

        int? providerId = null;
        if (args.Length >= 3 && int.TryParse(args[2], out var pid))
        {
            providerId = pid;
        }

        bool failedOnly = false;
        if (args.Length >= 4 && bool.TryParse(args[3], out var fo))
        {
            failedOnly = fo;
        }

        var client = _factory.CreateClient(protocol);

        await AnsiConsole.Status()
            .StartAsync($"Fetching fetch activity from {protocol}...", async ctx =>
            {
                var (data, metrics) = await client.GetFetchActivityAsync(count, providerId, failedOnly);

                DisplayUtilities.ShowMetrics(metrics, protocol.ToString().ToUpper());

                if (metrics.Success && data.Activities.Any())
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("[bold]Timestamp[/]")
                        .AddColumn("[bold]Provider[/]")
                        .AddColumn("[bold]Success[/]")
                        .AddColumn("[bold]Rates Count[/]")
                        .AddColumn("[bold]Duration (ms)[/]")
                        .AddColumn("[bold]Error[/]");

                    foreach (var activity in data.Activities)
                    {
                        table.AddRow(
                            Markup.Escape(activity.FetchedAt.ToString("yyyy-MM-dd HH:mm:ss")),
                            Markup.Escape(activity.ProviderCode ?? "-"),
                            activity.Success ? "[green]Yes[/]" : "[red]No[/]",
                            Markup.Escape((activity.RatesCount ?? 0).ToString()),
                            Markup.Escape(activity.DurationMs.ToString()),
                            Markup.Escape(activity.ErrorMessage ?? "-")
                        );
                    }

                    AnsiConsole.Write(table);
                    DisplayUtilities.ShowSuccess($"Total activities: {data.Activities.Count}");
                }
                else
                {
                    DisplayUtilities.ShowInfo("No fetch activity found");
                }
            });
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
        switch (input.ToLowerInvariant())
        {
            case "rest":
                protocol = ApiProtocol.Rest;
                return true;
            case "soap":
                protocol = ApiProtocol.Soap;
                return true;
            case "grpc":
                protocol = ApiProtocol.Grpc;
                return true;
            default:
                protocol = default;
                return false;
        }
    }
}
