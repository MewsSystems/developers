using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ConsoleTestApp.Config;
using ConsoleTestApp.Core;
using ConsoleTestApp.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleTestApp.Clients;

public class RestApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _hubUrl;
    private string _authToken = string.Empty;
    private HubConnection? _hubConnection;

    public string Protocol => "REST (HTTP/JSON)";
    public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);
    public bool SupportsStreaming => true;

    public RestApiClient(ApiEndpoints endpoints)
    {
        _baseUrl = endpoints.Rest;
        _hubUrl = endpoints.RestHub;
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        try
        {
            var loginRequest = new { email, password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    _authToken = result.Token;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                    return new AuthenticationResult
                    {
                        Success = true,
                        Token = result.Token,
                        Email = result.Email,
                        Role = result.Role,
                        ExpiresAt = result.ExpiresAt
                    };
                }
            }

            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"Login failed: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"Exception: {ex.Message}"
            };
        }
    }

    public Task LogoutAsync()
    {
        _authToken = string.Empty;
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return Task.CompletedTask;
    }

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRatesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/exchange-rates/latest");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestExchangeRateResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = MapToExchangeRateData(result);

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetHistoricalRatesAsync(DateTime from, DateTime to)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var url = $"/api/exchange-rates/historical?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";
            var response = await _httpClient.GetAsync(url);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestExchangeRateResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = MapToExchangeRateData(result);

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/exchange-rates/current");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestExchangeRateResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = MapToExchangeRateData(result);

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ConversionResult Data, ApiCallMetrics Metrics)> ConvertCurrencyAsync(string from, string to, decimal amount)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var url = $"/api/exchange-rates/convert?fromCurrency={from}&toCurrency={to}&amount={amount}";
            var response = await _httpClient.GetAsync(url);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestConversionResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ConversionResult
                {
                    FromCurrency = result?.Data?.SourceCurrencyCode ?? from,
                    ToCurrency = result?.Data?.TargetCurrencyCode ?? to,
                    Amount = amount,
                    ConvertedAmount = result?.Data?.TargetAmount ?? 0,
                    Rate = result?.Data?.EffectiveRate ?? 0,
                    ValidDate = DateTime.TryParse(result?.Data?.ValidDate, out var date) ? date : DateTime.UtcNow
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ConversionResult(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ConversionResult(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(CurrenciesListData Data, ApiCallMetrics Metrics)> GetCurrenciesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/currencies");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestCurrenciesResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new CurrenciesListData
                {
                    Currencies = result?.Data?.Select(c => new CurrencyData
                    {
                        Id = c.Id,
                        Code = c.Code ?? "",
                        Name = c.Name ?? "",
                        Symbol = c.Symbol,
                        DecimalPlaces = c.DecimalPlaces,
                        IsActive = c.IsActive
                    }).ToList() ?? new(),
                    TotalCount = result?.Data?.Count ?? 0
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new CurrenciesListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new CurrenciesListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/currencies/{code}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestCurrencyResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new CurrencyData
                {
                    Id = result?.Data?.Id ?? 0,
                    Code = result?.Data?.Code ?? "",
                    Name = result?.Data?.Name ?? "",
                    Symbol = result?.Data?.Symbol,
                    DecimalPlaces = result?.Data?.DecimalPlaces ?? 0,
                    IsActive = result?.Data?.IsActive ?? false
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new CurrencyData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new CurrencyData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ProvidersListData Data, ApiCallMetrics Metrics)> GetProvidersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/providers");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestProvidersResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ProvidersListData
                {
                    Providers = result?.Data?.Select(p => new ProviderData
                    {
                        Id = p.Id,
                        Code = p.Code ?? "",
                        Name = p.Name ?? "",
                        Description = p.Description,
                        BaseUrl = p.BaseUrl ?? "",
                        IsActive = p.IsActive,
                        CreatedAt = p.CreatedAt
                    }).ToList() ?? new(),
                    TotalCount = result?.Data?.Count ?? 0
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ProvidersListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProvidersListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/providers/{code}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestProviderResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ProviderData
                {
                    Id = result?.Data?.Id ?? 0,
                    Code = result?.Data?.Code ?? "",
                    Name = result?.Data?.Name ?? "",
                    Description = result?.Data?.Description,
                    BaseUrl = result?.Data?.BaseUrl ?? "",
                    IsActive = result?.Data?.IsActive ?? false,
                    CreatedAt = result?.Data?.CreatedAt ?? DateTime.UtcNow
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ProviderData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ProviderHealthData Data, ApiCallMetrics Metrics)> GetProviderHealthAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/providers/{code}/health");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestProviderHealthResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ProviderHealthData
                {
                    ProviderCode = result?.Data?.ProviderCode ?? code,
                    ProviderName = result?.Data?.ProviderName ?? "",
                    IsHealthy = result?.Data?.IsHealthy ?? false,
                    ConsecutiveFailures = result?.Data?.ConsecutiveFailures ?? 0,
                    LastSuccessfulFetch = result?.Data?.LastSuccessfulFetch,
                    LastFailedFetch = result?.Data?.LastFailedFetch,
                    LastError = result?.Data?.LastError
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ProviderHealthData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderHealthData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ProviderStatisticsData Data, ApiCallMetrics Metrics)> GetProviderStatisticsAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/providers/{code}/statistics");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestProviderStatsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ProviderStatisticsData
                {
                    ProviderCode = result?.Data?.ProviderCode ?? code,
                    ProviderName = result?.Data?.ProviderName ?? "",
                    TotalFetches = result?.Data?.TotalFetches ?? 0,
                    SuccessfulFetches = result?.Data?.SuccessfulFetches ?? 0,
                    FailedFetches = result?.Data?.FailedFetches ?? 0,
                    SuccessRate = result?.Data?.SuccessRate ?? 0,
                    TotalRatesProvided = result?.Data?.TotalRatesProvided ?? 0,
                    LastFetchAt = result?.Data?.LastFetchAt
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ProviderStatisticsData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderStatisticsData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/users");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestUsersResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new UsersListData
                {
                    Users = result?.Data?.Select(u => new UserData
                    {
                        Id = u.Id,
                        Email = u.Email ?? "",
                        FirstName = u.FirstName ?? "",
                        LastName = u.LastName ?? "",
                        Role = u.Role ?? "",
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt,
                        LastLoginAt = u.LastLoginAt
                    }).ToList() ?? new(),
                    TotalCount = result?.Data?.Count ?? 0
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new UsersListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new UsersListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(UserData Data, ApiCallMetrics Metrics)> GetUserAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/{id}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestUserResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new UserData
                {
                    Id = result?.Data?.Id ?? 0,
                    Email = result?.Data?.Email ?? "",
                    FirstName = result?.Data?.FirstName ?? "",
                    LastName = result?.Data?.LastName ?? "",
                    Role = result?.Data?.Role ?? "",
                    IsActive = result?.Data?.IsActive ?? false,
                    CreatedAt = result?.Data?.CreatedAt ?? DateTime.UtcNow,
                    LastLoginAt = result?.Data?.LastLoginAt
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new UserData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                PayloadSizeBytes = payloadSize,
                Success = false,
                ErrorMessage = $"HTTP {response.StatusCode}"
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new UserData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task StartStreamingAsync(Action<ExchangeRateData> onUpdate, CancellationToken cancellationToken)
    {
        if (_hubConnection != null)
        {
            await StopStreamingAsync();
        }

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_authToken)!;
            })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<object>("LatestRatesUpdated", (data) =>
        {
            // Parse SignalR event data and convert to ExchangeRateData
            var json = JsonSerializer.Serialize(data);
            var result = JsonSerializer.Deserialize<RestExchangeRateResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result != null)
            {
                onUpdate(MapToExchangeRateData(result));
            }
        });

        _hubConnection.On<object>("HistoricalRatesUpdated", (data) =>
        {
            var json = JsonSerializer.Serialize(data);
            var result = JsonSerializer.Deserialize<RestExchangeRateResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result != null)
            {
                onUpdate(MapToExchangeRateData(result));
            }
        });

        await _hubConnection.StartAsync(cancellationToken);
    }

    public async Task StopStreamingAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public async Task<bool> IsApiAvailableAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static ExchangeRateData MapToExchangeRateData(RestExchangeRateResponse? response)
    {
        if (response?.Data == null)
            return new ExchangeRateData();

        var data = new ExchangeRateData
        {
            FetchedAt = DateTime.UtcNow,
            Providers = new List<ProviderRates>()
        };

        foreach (var provider in response.Data.Providers ?? new List<RestProvider>())
        {
            var providerRates = new ProviderRates
            {
                ProviderCode = provider.ProviderCode ?? "",
                ProviderName = provider.ProviderName ?? "",
                BaseCurrencies = new List<BaseCurrencyRates>()
            };

            foreach (var baseCurrency in provider.BaseCurrencies ?? new List<RestBaseCurrency>())
            {
                var baseCurrencyRates = new BaseCurrencyRates
                {
                    CurrencyCode = baseCurrency.CurrencyCode ?? "",
                    TargetRates = new List<TargetRate>()
                };

                foreach (var target in baseCurrency.TargetCurrencies ?? new List<RestTargetCurrency>())
                {
                    baseCurrencyRates.TargetRates.Add(new TargetRate
                    {
                        CurrencyCode = target.CurrencyCode ?? "",
                        Rate = target.Rate,
                        Multiplier = target.Multiplier,
                        ValidDate = target.ValidDate
                    });
                }

                providerRates.BaseCurrencies.Add(baseCurrencyRates);
                data.TotalRates += baseCurrencyRates.TargetRates.Count;
            }

            data.Providers.Add(providerRates);
        }

        return data;
    }

    // REST API Response Models
    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    private class RestExchangeRateResponse
    {
        public RestDataWrapper? Data { get; set; }
    }

    private class RestDataWrapper
    {
        public List<RestProvider>? Providers { get; set; }
    }

    private class RestProvider
    {
        public string? ProviderCode { get; set; }
        public string? ProviderName { get; set; }
        public List<RestBaseCurrency>? BaseCurrencies { get; set; }
    }

    private class RestBaseCurrency
    {
        public string? CurrencyCode { get; set; }
        public List<RestTargetCurrency>? TargetCurrencies { get; set; }
    }

    private class RestTargetCurrency
    {
        public string? CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public int Multiplier { get; set; }
        public DateTime ValidDate { get; set; }
    }

    private class RestConversionResponse
    {
        public RestConversionData? Data { get; set; }
    }

    private class RestConversionData
    {
        public string? SourceCurrencyCode { get; set; }
        public string? TargetCurrencyCode { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal EffectiveRate { get; set; }
        public string? ValidDate { get; set; }
    }

    private class RestCurrenciesResponse
    {
        public List<RestCurrencyData>? Data { get; set; }
    }

    private class RestCurrencyResponse
    {
        public RestCurrencyData? Data { get; set; }
    }

    private class RestCurrencyData
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public bool IsActive { get; set; }
    }

    private class RestProvidersResponse
    {
        public List<RestProviderData>? Data { get; set; }
    }

    private class RestProviderResponse
    {
        public RestProviderData? Data { get; set; }
    }

    private class RestProviderData
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? BaseUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private class RestProviderHealthResponse
    {
        public RestProviderHealthData? Data { get; set; }
    }

    private class RestProviderHealthData
    {
        public string? ProviderCode { get; set; }
        public string? ProviderName { get; set; }
        public bool IsHealthy { get; set; }
        public int ConsecutiveFailures { get; set; }
        public DateTime? LastSuccessfulFetch { get; set; }
        public DateTime? LastFailedFetch { get; set; }
        public string? LastError { get; set; }
    }

    private class RestProviderStatsResponse
    {
        public RestProviderStatsData? Data { get; set; }
    }

    private class RestProviderStatsData
    {
        public string? ProviderCode { get; set; }
        public string? ProviderName { get; set; }
        public int TotalFetches { get; set; }
        public int SuccessfulFetches { get; set; }
        public int FailedFetches { get; set; }
        public double SuccessRate { get; set; }
        public int TotalRatesProvided { get; set; }
        public DateTime? LastFetchAt { get; set; }
    }

    private class RestUsersResponse
    {
        public List<RestUserData>? Data { get; set; }
    }

    private class RestUserResponse
    {
        public RestUserData? Data { get; set; }
    }

    private class RestUserData
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
