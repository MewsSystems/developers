using System.Diagnostics;
using ConsoleTestApp.Config;
using ConsoleTestApp.Core;
using Grpc.Core;
using Grpc.Net.Client;
using gRPC.Protos.Authentication;
using gRPC.Protos.Currencies;
using gRPC.Protos.ExchangeRates;
using gRPC.Protos.Providers;
using gRPC.Protos.Users;
using AppModels = ConsoleTestApp.Models;

namespace ConsoleTestApp.Clients;

public class GrpcApiClient : IApiClient
{
    private readonly GrpcChannel _channel;
    private readonly string _serverUrl;
    private string _authToken = string.Empty;
    private AsyncServerStreamingCall<ExchangeRateUpdateEvent>? _streamingCall;
    private CancellationTokenSource? _streamingCts;

    public string Protocol => "gRPC (HTTP/2 + Protobuf)";
    public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);
    public bool SupportsStreaming => true;

    public GrpcApiClient(ApiEndpoints endpoints)
    {
        _serverUrl = endpoints.Grpc;
        _channel = GrpcChannel.ForAddress(_serverUrl);
    }

    public async Task<AppModels.AuthenticationResult> LoginAsync(string email, string password)
    {
        try
        {
            var client = new AuthenticationService.AuthenticationServiceClient(_channel);
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await client.LoginAsync(request);

            if (response.Success && response.Data != null)
            {
                _authToken = response.Data.AccessToken;

                return new AppModels.AuthenticationResult
                {
                    Success = true,
                    Token = response.Data.AccessToken,
                    Email = response.Data.User?.Email ?? email,
                    Role = response.Data.User?.Role ?? "",
                    ExpiresAt = DateTime.UtcNow.AddSeconds(response.Data.ExpiresInSeconds)
                };
            }

            return new AppModels.AuthenticationResult
            {
                Success = false,
                ErrorMessage = response.Error?.Message ?? "Login failed"
            };
        }
        catch (RpcException ex)
        {
            return new AppModels.AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"gRPC Error: {ex.Status.Detail}"
            };
        }
        catch (Exception ex)
        {
            return new AppModels.AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"Exception: {ex.Message}"
            };
        }
    }

    public Task LogoutAsync()
    {
        _authToken = string.Empty;
        return Task.CompletedTask;
    }

    public async Task<(AppModels.ExchangeRateData Data, AppModels.ApiCallMetrics Metrics)> GetLatestRatesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
            var request = new GetAllLatestRatesRequest();

            var headers = CreateAuthHeaders();
            var response = await client.GetAllLatestRatesGroupedAsync(request, headers);

            stopwatch.Stop();

            var data = MapToExchangeRateData(response.Providers);

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(), // Protobuf size
                Success = true
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ExchangeRateData Data, AppModels.ApiCallMetrics Metrics)> GetHistoricalRatesAsync(DateTime from, DateTime to)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
            var request = new GetHistoryRequest
            {
                SourceCurrency = "EUR", // Default base currency for historical data
                TargetCurrency = "USD", // Default target currency
                StartDate = new gRPC.Protos.Common.Date
                {
                    Year = from.Year,
                    Month = from.Month,
                    Day = from.Day
                },
                EndDate = new gRPC.Protos.Common.Date
                {
                    Year = to.Year,
                    Month = to.Month,
                    Day = to.Day
                }
            };

            var headers = CreateAuthHeaders();
            var response = await client.GetHistoryGroupedAsync(request, headers);

            stopwatch.Stop();

            var data = MapToHistoricalExchangeRateData(response.Providers);

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(),
                Success = true
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task StartStreamingAsync(Action<AppModels.ExchangeRateData> onUpdate, CancellationToken cancellationToken)
    {
        if (_streamingCall != null)
        {
            await StopStreamingAsync();
        }

        _streamingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
        var request = new StreamSubscriptionRequest();
        request.SubscriptionTypes.Add("all"); // Subscribe to all updates

        var headers = CreateAuthHeaders();
        _streamingCall = client.StreamExchangeRateUpdates(request, headers, cancellationToken: _streamingCts.Token);

        // Start background task to read stream
        _ = Task.Run(async () =>
        {
            try
            {
                await foreach (var update in _streamingCall.ResponseStream.ReadAllAsync(_streamingCts.Token))
                {
                    var data = MapToExchangeRateData(update.Data.Providers);
                    onUpdate(data);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                // Stream was intentionally cancelled
            }
            catch (Exception)
            {
                // Stream error - connection lost
            }
        }, _streamingCts.Token);
    }

    public async Task StopStreamingAsync()
    {
        _streamingCts?.Cancel();
        _streamingCts?.Dispose();
        _streamingCts = null;

        if (_streamingCall != null)
        {
            _streamingCall.Dispose();
            _streamingCall = null;
        }

        await Task.CompletedTask;
    }

    public async Task<(AppModels.ExchangeRateData Data, AppModels.ApiCallMetrics Metrics)> GetCurrentRatesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
            var request = new GetCurrentRatesRequest();

            var headers = CreateAuthHeaders();
            var response = await client.GetCurrentRatesGroupedAsync(request, headers);

            stopwatch.Stop();

            var data = MapCurrentRatesToExchangeRateData(response.Providers);

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(),
                Success = true
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ConversionResult Data, AppModels.ApiCallMetrics Metrics)> ConvertCurrencyAsync(string from, string to, decimal amount)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
            var request = new ConvertCurrencyRequest
            {
                FromCurrency = from,
                ToCurrency = to,
                Amount = amount.ToString()
            };

            var headers = CreateAuthHeaders();
            var response = await client.ConvertCurrencyAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var data = new AppModels.ConversionResult
                {
                    FromCurrency = response.Data.SourceCurrencyCode,
                    ToCurrency = response.Data.TargetCurrencyCode,
                    Amount = decimal.Parse(response.Data.SourceAmount),
                    ConvertedAmount = decimal.Parse(response.Data.TargetAmount),
                    Rate = decimal.Parse(response.Data.EffectiveRate),
                    ValidDate = DateTime.Parse(response.Data.ValidDate)
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.ConversionResult(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ConversionResult(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ConversionResult(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.CurrenciesListData Data, AppModels.ApiCallMetrics Metrics)> GetCurrenciesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new CurrenciesService.CurrenciesServiceClient(_channel);
            var request = new GetAllCurrenciesRequest();

            var headers = CreateAuthHeaders();
            var response = await client.GetAllCurrenciesAsync(request, headers);

            stopwatch.Stop();

            var data = new AppModels.CurrenciesListData
            {
                Currencies = response.Currencies.Select(c => new AppModels.CurrencyData
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Symbol = c.Symbol,
                    DecimalPlaces = 2, // Default value as proto doesn't include this
                    IsActive = true // Default value as proto doesn't include this
                }).ToList()
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(),
                Success = true
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.CurrenciesListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.CurrenciesListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.CurrencyData Data, AppModels.ApiCallMetrics Metrics)> GetCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new CurrenciesService.CurrenciesServiceClient(_channel);
            var request = new GetCurrencyByCodeRequest { Code = code };

            var headers = CreateAuthHeaders();
            var response = await client.GetCurrencyByCodeAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var data = new AppModels.CurrencyData
                {
                    Id = response.Data.Id,
                    Code = response.Data.Code,
                    Name = response.Data.Name,
                    Symbol = response.Data.Symbol,
                    DecimalPlaces = 2, // Default value
                    IsActive = true // Default value
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.CurrencyData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.CurrencyData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.CurrencyData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ProvidersListData Data, AppModels.ApiCallMetrics Metrics)> GetProvidersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new GetAllProvidersRequest();

            var headers = CreateAuthHeaders();
            var response = await client.GetAllProvidersAsync(request, headers);

            stopwatch.Stop();

            var data = new AppModels.ProvidersListData
            {
                Providers = response.Providers.Select(p => new AppModels.ProviderData
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    BaseUrl = p.Url,
                    IsActive = p.IsActive,
                    CreatedAt = p.Created.ToDateTime()
                }).ToList()
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(),
                Success = true
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProvidersListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProvidersListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ProviderData Data, AppModels.ApiCallMetrics Metrics)> GetProviderAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new GetProviderByCodeRequest { Code = code };

            var headers = CreateAuthHeaders();
            var response = await client.GetProviderByCodeAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var data = new AppModels.ProviderData
                {
                    Id = response.Data.Id,
                    Code = response.Data.Code,
                    Name = response.Data.Name,
                    BaseUrl = response.Data.Url,
                    IsActive = response.Data.IsActive,
                    CreatedAt = response.Data.Created.ToDateTime()
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.ProviderData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ProviderHealthData Data, AppModels.ApiCallMetrics Metrics)> GetProviderHealthAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new GetProviderHealthRequest { Code = code };

            var headers = CreateAuthHeaders();
            var response = await client.GetProviderHealthAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var data = new AppModels.ProviderHealthData
                {
                    ProviderCode = response.Data.ProviderCode,
                    ProviderName = response.Data.ProviderName,
                    IsHealthy = response.Data.HealthStatus == "Healthy",
                    ConsecutiveFailures = response.Data.ConsecutiveFailures,
                    LastSuccessfulFetch = response.Data.LastSuccessfulFetch?.ToDateTime(),
                    LastFailedFetch = response.Data.LastFailedFetch?.ToDateTime(),
                    LastError = response.Data.LastErrorMessage
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.ProviderHealthData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderHealthData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderHealthData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ProviderStatisticsData Data, AppModels.ApiCallMetrics Metrics)> GetProviderStatisticsAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new GetProviderStatisticsRequest { Code = code };

            var headers = CreateAuthHeaders();
            var response = await client.GetProviderStatisticsAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var data = new AppModels.ProviderStatisticsData
                {
                    ProviderCode = response.Data.ProviderCode,
                    TotalFetches = (int)response.Data.TotalFetchAttempts,
                    SuccessfulFetches = (int)response.Data.SuccessfulFetches,
                    SuccessRate = response.Data.SuccessRate,
                    TotalRatesProvided = (int)response.Data.TotalRatesProvided
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.ProviderStatisticsData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderStatisticsData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderStatisticsData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.UsersListData Data, AppModels.ApiCallMetrics Metrics)> GetUsersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new GetAllUsersRequest();

            var headers = CreateAuthHeaders();
            var response = await client.GetAllUsersAsync(request, headers);

            stopwatch.Stop();

            var data = new AppModels.UsersListData
            {
                Users = response.Users.Select(u =>
                {
                    var nameParts = u.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    return new AppModels.UserData
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                        LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty,
                        Role = u.Role,
                        IsActive = true, // Default value as proto doesn't include this
                        CreatedAt = u.CreatedAt.ToDateTime()
                    };
                }).ToList()
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(),
                Success = true
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.UsersListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.UsersListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.UserData Data, AppModels.ApiCallMetrics Metrics)> GetUserAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new GetUserByIdRequest { Id = id };

            var headers = CreateAuthHeaders();
            var response = await client.GetUserByIdAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var nameParts = response.Data.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var data = new AppModels.UserData
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                    LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty,
                    Role = response.Data.Role,
                    IsActive = true, // Default value
                    CreatedAt = response.Data.CreatedAt.ToDateTime()
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.UserData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.UserData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.UserData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<bool> IsApiAvailableAsync()
    {
        try
        {
            var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
            var request = new GetCurrentRatesRequest();

            await client.GetCurrentRatesAsync(request, deadline: DateTime.UtcNow.AddSeconds(5));
            return true;
        }
        catch
        {
            return false;
        }
    }

    private Metadata CreateAuthHeaders()
    {
        var headers = new Metadata();
        if (!string.IsNullOrEmpty(_authToken))
        {
            headers.Add("Authorization", $"Bearer {_authToken}");
        }
        return headers;
    }

    private static AppModels.ExchangeRateData MapToExchangeRateData(Google.Protobuf.Collections.RepeatedField<LatestProviderRatesGroup> providers)
    {
        var data = new AppModels.ExchangeRateData
        {
            FetchedAt = DateTime.UtcNow,
            Providers = new List<AppModels.ProviderRates>()
        };

        foreach (var provider in providers)
        {
            var providerRates = new AppModels.ProviderRates
            {
                ProviderCode = provider.ProviderCode,
                ProviderName = provider.ProviderName,
                BaseCurrencies = new List<AppModels.BaseCurrencyRates>()
            };

            foreach (var baseCurrency in provider.BaseCurrencies)
            {
                var baseCurrencyRates = new AppModels.BaseCurrencyRates
                {
                    CurrencyCode = baseCurrency.BaseCurrencyCode,
                    TargetRates = new List<AppModels.TargetRate>()
                };

                foreach (var target in baseCurrency.TargetCurrencies)
                {
                    baseCurrencyRates.TargetRates.Add(new AppModels.TargetRate
                    {
                        CurrencyCode = target.TargetCurrencyCode,
                        Rate = decimal.Parse(target.Rate),
                        Multiplier = target.Multiplier,
                        ValidDate = target.ValidDate != null
                            ? new DateTime(target.ValidDate.Year, target.ValidDate.Month, target.ValidDate.Day)
                            : DateTime.UtcNow
                    });
                }

                providerRates.BaseCurrencies.Add(baseCurrencyRates);
                data.TotalRates += baseCurrencyRates.TargetRates.Count;
            }

            data.Providers.Add(providerRates);
        }

        return data;
    }

    private static AppModels.ExchangeRateData MapToHistoricalExchangeRateData(Google.Protobuf.Collections.RepeatedField<HistoryProviderGroup> providers)
    {
        var data = new AppModels.ExchangeRateData
        {
            FetchedAt = DateTime.UtcNow,
            Providers = new List<AppModels.ProviderRates>()
        };

        foreach (var provider in providers)
        {
            var providerRates = new AppModels.ProviderRates
            {
                ProviderCode = provider.ProviderCode,
                ProviderName = provider.ProviderName,
                BaseCurrencies = new List<AppModels.BaseCurrencyRates>()
            };

            foreach (var baseCurrency in provider.BaseCurrencies)
            {
                var baseCurrencyRates = new AppModels.BaseCurrencyRates
                {
                    CurrencyCode = baseCurrency.BaseCurrencyCode,
                    TargetRates = new List<AppModels.TargetRate>()
                };

                foreach (var target in baseCurrency.TargetCurrencies)
                {
                    // For historical data, we have multiple rates per target currency
                    foreach (var rate in target.Rates)
                    {
                        baseCurrencyRates.TargetRates.Add(new AppModels.TargetRate
                        {
                            CurrencyCode = target.TargetCurrencyCode,
                            Rate = decimal.Parse(rate.Rate),
                            Multiplier = rate.Multiplier,
                            ValidDate = rate.ValidDate != null
                                ? new DateTime(rate.ValidDate.Year, rate.ValidDate.Month, rate.ValidDate.Day)
                                : DateTime.UtcNow
                        });
                    }
                }

                providerRates.BaseCurrencies.Add(baseCurrencyRates);
                data.TotalRates += baseCurrencyRates.TargetRates.Count;
            }

            data.Providers.Add(providerRates);
        }

        return data;
    }

    private static AppModels.ExchangeRateData MapCurrentRatesToExchangeRateData(Google.Protobuf.Collections.RepeatedField<ProviderRatesGroup> providers)
    {
        var data = new AppModels.ExchangeRateData
        {
            FetchedAt = DateTime.UtcNow,
            Providers = new List<AppModels.ProviderRates>()
        };

        foreach (var provider in providers)
        {
            var providerRates = new AppModels.ProviderRates
            {
                ProviderCode = provider.ProviderCode,
                ProviderName = provider.ProviderName,
                BaseCurrencies = new List<AppModels.BaseCurrencyRates>()
            };

            foreach (var baseCurrency in provider.BaseCurrencies)
            {
                var baseCurrencyRates = new AppModels.BaseCurrencyRates
                {
                    CurrencyCode = baseCurrency.BaseCurrencyCode,
                    TargetRates = new List<AppModels.TargetRate>()
                };

                foreach (var target in baseCurrency.TargetCurrencies)
                {
                    baseCurrencyRates.TargetRates.Add(new AppModels.TargetRate
                    {
                        CurrencyCode = target.TargetCurrencyCode,
                        Rate = decimal.Parse(target.Rate),
                        Multiplier = target.Multiplier,
                        ValidDate = target.ValidDate != null
                            ? new DateTime(target.ValidDate.Year, target.ValidDate.Month, target.ValidDate.Day)
                            : DateTime.UtcNow
                    });
                }

                providerRates.BaseCurrencies.Add(baseCurrencyRates);
                data.TotalRates += baseCurrencyRates.TargetRates.Count;
            }

            data.Providers.Add(providerRates);
        }

        return data;
    }
}
