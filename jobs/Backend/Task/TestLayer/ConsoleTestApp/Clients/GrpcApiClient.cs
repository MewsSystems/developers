using System.Diagnostics;
using ConsoleTestApp.Config;
using ConsoleTestApp.Core;
using Grpc.Core;
using Grpc.Net.Client;
using gRPC.Protos.Authentication;
using gRPC.Protos.Currencies;
using gRPC.Protos.ExchangeRates;
using gRPC.Protos.Providers;
using gRPC.Protos.SystemHealth;
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
            var response = await client.GetCurrentRatesAsync(request, headers);

            stopwatch.Stop();

            // Map flat rate list to grouped structure
            var grouped = response.Rates
                .GroupBy(r => r.ProviderCode)
                .Select(providerGroup => new AppModels.ProviderRates
                {
                    ProviderCode = providerGroup.Key ?? "",
                    ProviderName = providerGroup.First().ProviderName ?? "",
                    BaseCurrencies = providerGroup
                        .GroupBy(r => r.SourceCurrencyCode)
                        .Select(baseGroup => new AppModels.BaseCurrencyRates
                        {
                            CurrencyCode = baseGroup.Key ?? "",
                            TargetRates = baseGroup.Select(r => new AppModels.TargetRate
                            {
                                CurrencyCode = r.TargetCurrencyCode ?? "",
                                Rate = decimal.TryParse(r.EffectiveRate, out var rate) ? rate : 0m,
                                Multiplier = r.Multiplier,
                                ValidDate = r.ValidDate != null ? new DateTime(r.ValidDate.Year, r.ValidDate.Month, r.ValidDate.Day) : DateTime.UtcNow
                            }).ToList()
                        }).ToList()
                }).ToList();

            var data = new AppModels.ExchangeRateData
            {
                Providers = grouped,
                FetchedAt = DateTime.UtcNow,
                TotalRates = response.Rates.Count
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

    public async Task<(AppModels.ExchangeRateData Data, AppModels.ApiCallMetrics Metrics)> GetCurrentRatesGroupedAsync()
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

    public async Task<(AppModels.ExchangeRateData Data, AppModels.ApiCallMetrics Metrics)> GetLatestRateAsync(string sourceCurrency, string targetCurrency, int? providerId = null)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ExchangeRatesService.ExchangeRatesServiceClient(_channel);
            var request = new GetLatestRateRequest
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency
            };

            if (providerId.HasValue)
            {
                request.ProviderId = providerId.Value;
            }

            var headers = CreateAuthHeaders();
            var response = await client.GetLatestRateAsync(request, headers);

            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                var data = new AppModels.ExchangeRateData
                {
                    Providers = new List<AppModels.ProviderRates>
                    {
                        new AppModels.ProviderRates
                        {
                            ProviderCode = response.Data.ProviderCode ?? "Unknown",
                            ProviderName = response.Data.ProviderCode ?? "Unknown",
                            BaseCurrencies = new List<AppModels.BaseCurrencyRates>
                            {
                                new AppModels.BaseCurrencyRates
                                {
                                    CurrencyCode = sourceCurrency,
                                    TargetRates = new List<AppModels.TargetRate>
                                    {
                                        new AppModels.TargetRate
                                        {
                                            CurrencyCode = targetCurrency,
                                            Rate = decimal.TryParse(response.Data.EffectiveRate, out var rate) ? rate : 0m,
                                            Multiplier = response.Data.Multiplier,
                                            ValidDate = response.Data.ValidDate != null ? new DateTime(response.Data.ValidDate.Year, response.Data.ValidDate.Month, response.Data.ValidDate.Day) : DateTime.UtcNow
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                return (data, new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = response.CalculateSize(),
                    Success = true
                });
            }

            return (new AppModels.ExchangeRateData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = response.Message
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

    public async Task<(AppModels.CurrencyData Data, AppModels.ApiCallMetrics Metrics)> GetCurrencyAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new CurrenciesService.CurrenciesServiceClient(_channel);
            var request = new GetCurrencyByIdRequest { Id = id };

            var headers = CreateAuthHeaders();
            var response = await client.GetCurrencyByIdAsync(request, headers);

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

    public async Task<(AppModels.CurrencyData Data, AppModels.ApiCallMetrics Metrics)> GetCurrencyByCodeAsync(string code)
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
                    CreatedAt = p.Created?.ToDateTime() ?? DateTime.MinValue
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

    public async Task<(AppModels.ProviderData Data, AppModels.ApiCallMetrics Metrics)> GetProviderAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new GetProviderByIdRequest { Id = id };

            var headers = CreateAuthHeaders();
            var response = await client.GetProviderByIdAsync(request, headers);

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
                    CreatedAt = response.Data.Created?.ToDateTime() ?? DateTime.MinValue
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

    public async Task<(AppModels.ProviderData Data, AppModels.ApiCallMetrics Metrics)> GetProviderByCodeAsync(string code)
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
                    CreatedAt = response.Data.Created?.ToDateTime() ?? DateTime.MinValue
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

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> RescheduleProviderAsync(string code, string updateTime, string timeZone)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new RescheduleProviderRequest
            {
                Code = code,
                UpdateTime = updateTime,
                TimeZone = timeZone
            };

            var headers = CreateAuthHeaders();
            var response = await client.RescheduleProviderAsync(request, headers);

            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message,
                ErrorMessage = response.Success ? string.Empty : response.Message
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = response.CalculateSize(),
                Success = response.Success
            });
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}" },
                new AppModels.ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = $"gRPC {ex.Status.StatusCode}: {ex.Status.Detail}"
                });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message },
                new AppModels.ApiCallMetrics
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
                    var nameParts = string.IsNullOrEmpty(u.FullName)
                        ? Array.Empty<string>()
                        : u.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    return new AppModels.UserData
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                        LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty,
                        Role = u.Role,
                        IsActive = true, // Default value as proto doesn't include this
                        CreatedAt = u.CreatedAt?.ToDateTime() ?? DateTime.MinValue
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
                var nameParts = string.IsNullOrEmpty(response.Data.FullName)
                    ? Array.Empty<string>()
                    : response.Data.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var data = new AppModels.UserData
                {
                    Id = response.Data.Id,
                    Email = response.Data.Email,
                    FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty,
                    LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty,
                    Role = response.Data.Role,
                    IsActive = true, // Default value
                    CreatedAt = response.Data.CreatedAt?.ToDateTime() ?? DateTime.MinValue
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

    // Provider management operations
    public async Task<(AppModels.ProviderConfigurationData Data, AppModels.ApiCallMetrics Metrics)> GetProviderConfigurationAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new GetProviderConfigurationRequest { Code = code };
            var response = await client.GetProviderConfigurationAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.ProviderConfigurationData
            {
                Id = response.Data?.Id ?? 0,
                Code = response.Data?.Code ?? "",
                Name = response.Data?.Name ?? "",
                Url = response.Data?.Url ?? "",
                IsActive = response.Data?.IsActive ?? false,
                BaseCurrencyCode = response.Data?.BaseCurrencyCode,
                RequiresAuthentication = response.Data?.RequiresAuthentication ?? false,
                ApiKeyVaultReference = response.Data?.ApiKeyVaultReference,
                CreatedAt = response.Data?.Created?.ToDateTime() ?? DateTime.UtcNow,
                LastModifiedAt = response.Data?.Modified?.ToDateTime()
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success,
                ErrorMessage = response.Success ? "" : response.Message
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ProviderConfigurationData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> ActivateProviderAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new ActivateProviderRequest { Code = code };
            var response = await client.ActivateProviderAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> DeactivateProviderAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new DeactivateProviderRequest { Code = code };
            var response = await client.DeactivateProviderAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> ResetProviderHealthAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new ResetProviderHealthRequest { Code = code };
            var response = await client.ResetProviderHealthAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> TriggerManualFetchAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new TriggerManualFetchRequest { Code = code };
            var response = await client.TriggerManualFetchAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> CreateProviderAsync(string name, string code, string url, int baseCurrencyId, bool requiresAuth, string? apiKeyRef)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new CreateProviderRequest
            {
                Name = name,
                Code = code,
                Url = url,
                BaseCurrencyId = baseCurrencyId,
                RequiresAuthentication = requiresAuth,
                ApiKeyVaultReference = apiKeyRef ?? ""
            };
            var response = await client.CreateProviderAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> UpdateProviderConfigurationAsync(string code, string name, string url, bool requiresAuth, string? apiKeyRef)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new UpdateProviderConfigurationRequest
            {
                Code = code,
                Name = name,
                Url = url,
                RequiresAuthentication = requiresAuth,
                ApiKeyVaultReference = apiKeyRef ?? ""
            };
            var response = await client.UpdateProviderConfigurationAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> DeleteProviderAsync(string code, bool force)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new ProvidersService.ProvidersServiceClient(_channel);
            var request = new DeleteProviderRequest { Code = code, Force = force };
            var response = await client.DeleteProviderAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    // User management operations
    public async Task<(AppModels.UserData Data, AppModels.ApiCallMetrics Metrics)> GetUserByEmailAsync(string email)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new GetUserByEmailRequest { Email = email };
            var response = await client.GetUserByEmailAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.UserData
            {
                Id = response.Data?.Id ?? 0,
                Email = response.Data?.Email ?? "",
                FirstName = response.Data?.FullName?.Split(' ').FirstOrDefault() ?? "",
                LastName = response.Data?.FullName?.Contains(' ') == true ? string.Join(" ", response.Data.FullName.Split(' ').Skip(1)) : "",
                Role = response.Data?.Role ?? "",
                IsActive = true,
                CreatedAt = response.Data?.CreatedAt?.ToDateTime() ?? DateTime.UtcNow
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
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

    public async Task<(AppModels.UsersListData Data, AppModels.ApiCallMetrics Metrics)> GetUsersByRoleAsync(string role)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new GetUsersByRoleRequest { Role = role };
            var response = await client.GetUsersByRoleAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.UsersListData
            {
                Users = response.Users?.Select(u => new AppModels.UserData
                {
                    Id = u.Id,
                    Email = u.Email ?? "",
                    FirstName = u.FullName?.Split(' ').FirstOrDefault() ?? "",
                    LastName = u.FullName?.Contains(' ') == true ? string.Join(" ", u.FullName.Split(' ').Skip(1)) : "",
                    Role = u.Role ?? "",
                    IsActive = true,
                    CreatedAt = u.CreatedAt?.ToDateTime() ?? DateTime.UtcNow
                }).ToList() ?? new List<AppModels.UserData>()
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = true
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

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> CheckEmailExistsAsync(string email)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new CheckEmailExistsRequest { Email = email };
            var response = await client.CheckEmailExistsAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            return (new AppModels.OperationResult
            {
                Success = response.Exists,
                Message = response.Message ?? (response.Exists ? "Email exists" : "Email does not exist")
            }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = true
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult
            {
                Success = false,
                Message = ex.Message
            }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> CreateUserAsync(string email, string password, string firstName, string lastName, string role)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new CreateUserRequest
            {
                Email = email,
                Password = password,
                FullName = $"{firstName} {lastName}",
                Role = role
            };
            var response = await client.CreateUserAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> UpdateUserAsync(int id, string firstName, string lastName)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new UpdateUserInfoRequest
            {
                Id = id,
                FullName = $"{firstName} {lastName}",
                Email = "" // Email not being updated
            };
            var response = await client.UpdateUserInfoAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> ChangePasswordAsync(int id, string currentPassword, string newPassword)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new ChangeUserPasswordRequest
            {
                Id = id,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };
            var response = await client.ChangeUserPasswordAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> ChangeUserRoleAsync(int id, string newRole)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new ChangeUserRoleRequest
            {
                Id = id,
                NewRole = newRole
            };
            var response = await client.ChangeUserRoleAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> DeleteUserAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new UsersService.UsersServiceClient(_channel);
            var request = new DeleteUserRequest { Id = id };
            var response = await client.DeleteUserAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    // Currency management operations
    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> CreateCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new CurrenciesService.CurrenciesServiceClient(_channel);
            var request = new CreateCurrencyRequest
            {
                Code = code,
                Name = code, // Default to code as name
                Symbol = code // Default to code as symbol
            };
            var response = await client.CreateCurrencyAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.OperationResult Data, AppModels.ApiCallMetrics Metrics)> DeleteCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new CurrenciesService.CurrenciesServiceClient(_channel);
            var request = new DeleteCurrencyRequest { Code = code };
            var response = await client.DeleteCurrencyAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.OperationResult
            {
                Success = response.Success,
                Message = response.Message ?? "",
                ErrorMessage = response.Success ? "" : (response.Message ?? "")
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = response.Success
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.OperationResult { Success = false, ErrorMessage = ex.Message }, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    // System Health operations
    public async Task<(AppModels.SystemHealthData Data, AppModels.ApiCallMetrics Metrics)> GetSystemHealthAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new SystemHealthService.SystemHealthServiceClient(_channel);
            var request = new GetSystemHealthRequest();
            var response = await client.GetSystemHealthAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.SystemHealthData
            {
                Status = "Healthy",
                TotalProviders = response.Health?.TotalProviders ?? 0,
                HealthyProviders = response.Health?.ActiveProviders ?? 0,
                UnhealthyProviders = response.Health?.QuarantinedProviders ?? 0,
                TotalCurrencies = response.Health?.TotalCurrencies ?? 0,
                TotalUsers = 0, // Not provided in gRPC response
                TotalExchangeRates = response.Health?.TotalExchangeRates ?? 0,
                SystemUptime = 0 // Not provided in gRPC response
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = true
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.SystemHealthData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.ErrorsListData Data, AppModels.ApiCallMetrics Metrics)> GetRecentErrorsAsync(int count, string? severity)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new SystemHealthService.SystemHealthServiceClient(_channel);
            var request = new GetRecentErrorsRequest { Limit = count };
            var response = await client.GetRecentErrorsAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var data = new AppModels.ErrorsListData
            {
                Errors = response.Errors.Select(e => new AppModels.ErrorSummaryData
                {
                    Id = (int)e.Id,
                    ErrorMessage = e.ErrorMessage ?? "",
                    Severity = e.ErrorType ?? "",
                    SourceComponent = "",
                    OccurredAt = e.OccurredAt?.ToDateTime() ?? DateTime.UtcNow,
                    ProviderId = e.ProviderId,
                    ProviderCode = e.ProviderCode
                }).ToList(),
                TotalCount = response.Errors.Count
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = true
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.ErrorsListData(), new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(AppModels.FetchActivityListData Data, AppModels.ApiCallMetrics Metrics)> GetFetchActivityAsync(int count, int? providerId, bool failedOnly)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var client = new SystemHealthService.SystemHealthServiceClient(_channel);
            var request = new GetFetchActivityRequest { Limit = count };
            if (providerId.HasValue)
            {
                request.ProviderId = providerId.Value;
            }
            var response = await client.GetFetchActivityAsync(request, headers: CreateAuthHeaders());
            stopwatch.Stop();

            var activities = response.FetchLogs
                .Where(f => !failedOnly || f.Status == "Failed")
                .Select(f => new AppModels.FetchActivityData
                {
                    Id = (int)f.Id,
                    ProviderId = f.ProviderId,
                    ProviderCode = f.ProviderCode ?? "",
                    ProviderName = f.ProviderCode ?? "",
                    FetchedAt = f.StartedAt?.ToDateTime() ?? DateTime.UtcNow,
                    Success = f.Status == "Success",
                    RatesCount = f.RecordsFetched,
                    ErrorMessage = f.ErrorMessage,
                    DurationMs = 0 // Calculate if possible
                }).ToList();

            var data = new AppModels.FetchActivityListData
            {
                Activities = activities,
                TotalCount = activities.Count
            };

            return (data, new AppModels.ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = true
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new AppModels.FetchActivityListData(), new AppModels.ApiCallMetrics
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
            // Simple check: try to make an HTTP request to the gRPC endpoint
            // gRPC services respond to HTTP/2 requests
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
            var response = await httpClient.GetAsync(_serverUrl);
            // gRPC endpoints return various responses for HTTP GET, but if they respond, they're alive
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
                        Rate = decimal.Parse(target.EffectiveRate),
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
                            Rate = decimal.Parse(rate.EffectiveRate),
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
                        Rate = decimal.Parse(target.EffectiveRate),
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
