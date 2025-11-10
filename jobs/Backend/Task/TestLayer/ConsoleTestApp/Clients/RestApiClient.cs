using ConsoleTestApp.Config;
using ConsoleTestApp.Core;
using ConsoleTestApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
                if (result != null && result.Data != null)
                {
                    _authToken = result.Data.AccessToken;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                    return new AuthenticationResult
                    {
                        Success = true,
                        Token = result.Data.AccessToken,
                        Email = result.Data.Email,
                        Role = result.Data.Role,
                        ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(result.Data.ExpiresAt).DateTime
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
            // Debug: Check if auth header is still set
            var response = await _httpClient.GetAsync("/api/exchange-rates/latest/all/grouped");
            stopwatch.Stop();

            // Debug: Log response status

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
            var url = $"/api/exchange-rates/history/grouped?sourceCurrency=EUR&targetCurrency=USD&startDate={from:yyyy-MM-dd}&endDate={to:yyyy-MM-dd}";
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
            var response = await _httpClient.GetAsync("/api/exchange-rates/current/grouped");
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

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesGroupedAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/exchange-rates/current/grouped");
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

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRateAsync(string sourceCurrency, string targetCurrency, int? providerId = null)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var url = $"/api/exchange-rates/latest?sourceCurrency={sourceCurrency}&targetCurrency={targetCurrency}";
            if (providerId.HasValue)
            {
                url += $"&providerId={providerId.Value}";
            }

            var response = await _httpClient.GetAsync(url);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestSingleExchangeRateResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = MapSingleExchangeRateToData(result?.Data);

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

    public async Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/currencies/id/{id}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestCurrencyResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Data != null)
                {
                    var data = new CurrencyData
                    {
                        Id = result.Data.Id,
                        Code = result?.Data?.Code != null ? result.Data.Code : "",
                        Name = result?.Data?.Name != null ? result.Data.Name : "",
                        Symbol = result?.Data?.Symbol != null ? result.Data.Symbol : "",
                        DecimalPlaces = 2, // Default
                        IsActive = true // Default
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = payloadSize,
                        Success = true
                    });
                }
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

    public async Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyByCodeAsync(string code)
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

    public async Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/providers/id/{id}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestProviderDetailResponse>(content, new JsonSerializerOptions
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

    public async Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderByCodeAsync(string code)
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

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> RescheduleProviderAsync(string code, string updateTime, string timeZone)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new { UpdateTime = updateTime, TimeZone = timeZone };
            var response = await _httpClient.PostAsJsonAsync($"/api/providers/{code}/reschedule", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = result?.Success ?? true,
                    Message = result?.Message ?? $"Provider {code} rescheduled successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ProviderConfigurationData Data, ApiCallMetrics Metrics)> GetProviderConfigurationAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/providers/{code}/configuration");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestProviderConfigResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ProviderConfigurationData
                {
                    Id = result?.Data?.Id ?? 0,
                    Code = result?.Data?.Code ?? "",
                    Name = result?.Data?.Name ?? "",
                    Description = result?.Data?.Description,
                    Url = result?.Data?.Url ?? "",
                    IsActive = result?.Data?.IsActive ?? false,
                    BaseCurrencyId = result?.Data?.BaseCurrencyId ?? 0,
                    BaseCurrencyCode = result?.Data?.BaseCurrencyCode,
                    RequiresAuthentication = result?.Data?.RequiresAuthentication ?? false,
                    ApiKeyVaultReference = result?.Data?.ApiKeyVaultReference,
                    CreatedAt = result?.Data?.CreatedAt ?? DateTime.UtcNow,
                    LastModifiedAt = result?.Data?.LastModifiedAt
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ProviderConfigurationData(), new ApiCallMetrics
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
            return (new ProviderConfigurationData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ActivateProviderAsync(string code)
    {
        return await ExecuteProviderOperationAsync($"/api/providers/{code}/activate", $"Provider {code} activated successfully");
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeactivateProviderAsync(string code)
    {
        return await ExecuteProviderOperationAsync($"/api/providers/{code}/deactivate", $"Provider {code} deactivated successfully");
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ResetProviderHealthAsync(string code)
    {
        return await ExecuteProviderOperationAsync($"/api/providers/{code}/reset-health", $"Provider {code} health reset successfully");
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> TriggerManualFetchAsync(string code)
    {
        return await ExecuteProviderOperationAsync($"/api/providers/{code}/fetch", $"Manual fetch triggered for provider {code}");
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateProviderAsync(string name, string code, string url, int baseCurrencyId, bool requiresAuth, string? apiKeyRef)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new
            {
                Name = name,
                Code = code,
                Url = url,
                BaseCurrencyId = baseCurrencyId,
                RequiresAuthentication = requiresAuth,
                ApiKeyVaultReference = apiKeyRef
            };
            var response = await _httpClient.PostAsJsonAsync("/api/providers", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Provider {code} created successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> UpdateProviderConfigurationAsync(string code, string name, string url, bool requiresAuth, string? apiKeyRef)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new
            {
                Name = name,
                Url = url,
                RequiresAuthentication = requiresAuth,
                ApiKeyVaultReference = apiKeyRef
            };
            var response = await _httpClient.PutAsJsonAsync($"/api/providers/{code}/configuration", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Provider {code} configuration updated successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteProviderAsync(string code, bool force)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/providers/{code}?force={force}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Provider {code} deleted successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    private async Task<(OperationResult Data, ApiCallMetrics Metrics)> ExecuteProviderOperationAsync(string endpoint, string successMessage)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.PostAsync(endpoint, null);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? successMessage
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
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
                    TotalCount = result?.TotalCount ?? 0
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

    public async Task<(UserData Data, ApiCallMetrics Metrics)> GetUserByEmailAsync(string email)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/by-email/{email}");
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

    public async Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersByRoleAsync(string role)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/by-role/{role}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestUsersListResponse>(content, new JsonSerializerOptions
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
                    }).ToList() ?? new List<UserData>()
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

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CheckEmailExistsAsync(string email)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/check-email/{email}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestCheckEmailResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return (new OperationResult
                {
                    Success = result?.Data ?? false,
                    Message = result?.Message ?? (result?.Data == true ? "Email exists" : "Email does not exist")
                }, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult
            {
                Success = false,
                Message = $"HTTP {response.StatusCode}"
            }, new ApiCallMetrics
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
            return (new OperationResult
            {
                Success = false,
                Message = ex.Message
            }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateUserAsync(string email, string password, string firstName, string lastName, string role)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Role = role
            };
            var response = await _httpClient.PostAsJsonAsync("/api/users", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"User {email} created successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> UpdateUserAsync(int id, string firstName, string lastName)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new
            {
                FirstName = firstName,
                LastName = lastName
            };
            var response = await _httpClient.PutAsJsonAsync($"/api/users/{id}", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"User {id} updated successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ChangePasswordAsync(int id, string currentPassword, string newPassword)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };
            var response = await _httpClient.PostAsJsonAsync($"/api/users/{id}/change-password", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Password for user {id} changed successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ChangeUserRoleAsync(int id, string newRole)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new { NewRole = newRole };
            var response = await _httpClient.PostAsJsonAsync($"/api/users/{id}/change-role", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Role for user {id} changed to {newRole}"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteUserAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{id}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"User {id} deleted successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var request = new { Code = code };
            var response = await _httpClient.PostAsJsonAsync("/api/currencies", request);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Currency {code} created successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/currencies/{code}");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestOperationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new OperationResult
                {
                    Success = true,
                    Message = result?.Message ?? $"Currency {code} deleted successfully"
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new OperationResult { Success = false, ErrorMessage = $"HTTP {response.StatusCode}" }, new ApiCallMetrics
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
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(SystemHealthData Data, ApiCallMetrics Metrics)> GetSystemHealthAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await _httpClient.GetAsync("/api/system-health");
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestSystemHealthResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new SystemHealthData
                {
                    Status = result?.Data?.Status ?? "",
                    TotalProviders = result?.Data?.TotalProviders ?? 0,
                    HealthyProviders = result?.Data?.HealthyProviders ?? 0,
                    UnhealthyProviders = result?.Data?.UnhealthyProviders ?? 0,
                    TotalCurrencies = result?.Data?.TotalCurrencies ?? 0,
                    TotalUsers = result?.Data?.TotalUsers ?? 0,
                    LastFetchTime = result?.Data?.LastFetchTime,
                    TotalExchangeRates = result?.Data?.TotalExchangeRates ?? 0,
                    SystemUptime = result?.Data?.SystemUptime ?? 0
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new SystemHealthData(), new ApiCallMetrics
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
            return (new SystemHealthData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ErrorsListData Data, ApiCallMetrics Metrics)> GetRecentErrorsAsync(int count, string? severity)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var url = $"/api/system-health/errors?count={count}";
            if (!string.IsNullOrEmpty(severity))
            {
                url += $"&severity={severity}";
            }
            var response = await _httpClient.GetAsync(url);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestErrorsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new ErrorsListData
                {
                    Errors = result?.Data?.Select(e => new ErrorSummaryData
                    {
                        Id = e.Id,
                        ErrorMessage = e.ErrorMessage ?? "",
                        Severity = e.Severity,
                        SourceComponent = e.SourceComponent,
                        OccurredAt = e.OccurredAt,
                        ProviderId = e.ProviderId,
                        ProviderCode = e.ProviderCode
                    }).ToList() ?? new(),
                    TotalCount = result?.Data?.Count() ?? 0
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new ErrorsListData(), new ApiCallMetrics
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
            return (new ErrorsListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(FetchActivityListData Data, ApiCallMetrics Metrics)> GetFetchActivityAsync(int count, int? providerId, bool failedOnly)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var url = $"/api/system-health/fetch-activity?count={count}&failedOnly={failedOnly}";
            if (providerId.HasValue)
            {
                url += $"&providerId={providerId.Value}";
            }
            var response = await _httpClient.GetAsync(url);
            stopwatch.Stop();

            var content = await response.Content.ReadAsStringAsync();
            var payloadSize = Encoding.UTF8.GetByteCount(content);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RestFetchActivityResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var data = new FetchActivityListData
                {
                    Activities = result?.Data?.Select(f => new FetchActivityData
                    {
                        Id = f.Id,
                        ProviderId = f.ProviderId,
                        ProviderCode = f.ProviderCode ?? "",
                        ProviderName = f.ProviderName ?? "",
                        FetchedAt = f.FetchedAt,
                        Success = f.Success,
                        RatesCount = f.RatesCount,
                        ErrorMessage = f.ErrorMessage,
                        DurationMs = f.DurationMs
                    }).ToList() ?? new(),
                    TotalCount = result?.Data?.Count() ?? 0
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = payloadSize,
                    Success = true
                });
            }

            return (new FetchActivityListData(), new ApiCallMetrics
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
            return (new FetchActivityListData(), new ApiCallMetrics
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
            // Simple check: just verify the server responds to any request
            using var testClient = new HttpClient { BaseAddress = _httpClient.BaseAddress, Timeout = TimeSpan.FromSeconds(2) };
            var response = await testClient.GetAsync("/");
            // Server is alive if it responds (even with 404 or redirect)
            return true;
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

        foreach (var provider in response.Data ?? new List<RestProvider>())
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

    private static ExchangeRateData MapSingleExchangeRateToData(RestSingleExchangeRateData? exchangeRate)
    {
        if (exchangeRate == null)
            return new ExchangeRateData();

        var data = new ExchangeRateData
        {
            FetchedAt = DateTime.UtcNow,
            Providers = new List<ProviderRates>()
        };

        var providerRates = new ProviderRates
        {
            ProviderCode = exchangeRate.Provider?.Code ?? "",
            ProviderName = exchangeRate.Provider?.Name ?? "",
            BaseCurrencies = new List<BaseCurrencyRates>()
        };

        var baseCurrencyRates = new BaseCurrencyRates
        {
            CurrencyCode = exchangeRate.CurrencyPair?.SourceCurrencyCode ?? "",
            TargetRates = new List<TargetRate>()
        };

        baseCurrencyRates.TargetRates.Add(new TargetRate
        {
            CurrencyCode = exchangeRate.CurrencyPair?.TargetCurrencyCode ?? "",
            Rate = exchangeRate.RateInfo?.EffectiveRate ?? 0,
            Multiplier = exchangeRate.RateInfo?.Multiplier ?? 1,
            ValidDate = DateTime.TryParse(exchangeRate.ValidDate, out var validDate)
                ? validDate
                : DateTime.UtcNow
        });

        providerRates.BaseCurrencies.Add(baseCurrencyRates);
        data.Providers.Add(providerRates);
        data.TotalRates = 1;

        return data;
    }

    // REST API Response Models
    private class LoginResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public LoginData? Data { get; set; }
    }

    private class LoginData
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public long ExpiresAt { get; set; }
    }

    private class RestExchangeRateResponse
    {
        public List<RestProvider>? Data { get; set; }
    }

    // Response models for single exchange rate endpoint (GET /api/exchange-rates/latest?sourceCurrency=...&targetCurrency=...)
    private class RestSingleExchangeRateResponse
    {
        public RestSingleExchangeRateData? Data { get; set; }
    }

    private class RestSingleExchangeRateData
    {
        public int Id { get; set; }
        public RestProviderInfo? Provider { get; set; }
        public RestCurrencyPair? CurrencyPair { get; set; }
        public RestRateInfo? RateInfo { get; set; }
        public string? ValidDate { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }

    private class RestCurrencyPair
    {
        public int SourceCurrencyId { get; set; }
        public string? SourceCurrencyCode { get; set; }
        public int TargetCurrencyId { get; set; }
        public string? TargetCurrencyCode { get; set; }
    }

    // Note: RestProvider now maps to the grouped response structure (CurrentExchangeRatesGroupedResponse, etc.)

    private class RestProvider
    {
        public RestProviderInfo? Provider { get; set; }
        public List<RestBaseCurrency>? BaseCurrencies { get; set; }

        // Computed properties for backwards compatibility
        public string? ProviderCode => Provider?.Code;
        public string? ProviderName => Provider?.Name;
    }

    private class RestProviderInfo
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    private class RestBaseCurrency
    {
        public string? BaseCurrency { get; set; }
        public List<RestTargetCurrency>? Rates { get; set; }

        // Computed property for backwards compatibility
        public string? CurrencyCode => BaseCurrency;
        public List<RestTargetCurrency>? TargetCurrencies => Rates;
    }

    private class RestTargetCurrency
    {
        public string? TargetCurrency { get; set; }
        public RestRateInfo? RateInfo { get; set; }
        public DateTime ValidDate { get; set; }

        // Computed properties for backwards compatibility
        public string? CurrencyCode => TargetCurrency;
        public decimal Rate => RateInfo?.EffectiveRate ?? 0;
        public int Multiplier => RateInfo?.Multiplier ?? 1;
    }

    private class RestRateInfo
    {
        public decimal RawRate { get; set; }
        public int Multiplier { get; set; }
        public decimal EffectiveRate { get; set; }
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
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
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

    private class RestOperationResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    private class RestProviderConfigResponse
    {
        public RestProviderConfigData? Data { get; set; }
    }

    private class RestProviderConfigData
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public bool IsActive { get; set; }
        public int BaseCurrencyId { get; set; }
        public string? BaseCurrencyCode { get; set; }
        public bool RequiresAuthentication { get; set; }
        public string? ApiKeyVaultReference { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    private class RestSystemHealthResponse
    {
        public RestSystemHealthData? Data { get; set; }
    }

    private class RestSystemHealthData
    {
        public string? Status { get; set; }
        public int TotalProviders { get; set; }
        public int HealthyProviders { get; set; }
        public int UnhealthyProviders { get; set; }
        public int TotalCurrencies { get; set; }
        public int TotalUsers { get; set; }
        public DateTime? LastFetchTime { get; set; }
        public long TotalExchangeRates { get; set; }
        public double SystemUptime { get; set; }
    }

    private class RestErrorsResponse
    {
        public List<RestErrorData>? Data { get; set; }
    }

    private class RestErrorData
    {
        public int Id { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Severity { get; set; }
        public string? SourceComponent { get; set; }
        public DateTime OccurredAt { get; set; }
        public int? ProviderId { get; set; }
        public string? ProviderCode { get; set; }
    }

    private class RestFetchActivityResponse
    {
        public List<RestFetchActivityData>? Data { get; set; }
    }

    private class RestFetchActivityData
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string? ProviderCode { get; set; }
        public string? ProviderName { get; set; }
        public DateTime FetchedAt { get; set; }
        public bool Success { get; set; }
        public int? RatesCount { get; set; }
        public string? ErrorMessage { get; set; }
        public long DurationMs { get; set; }
    }

    private class RestProviderDetailResponse
    {
        public RestProviderDetailData? Data { get; set; }
    }

    private class RestProviderDetailData
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? BaseUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private class RestUsersListResponse
    {
        public List<RestUserData>? Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    private class RestCheckEmailResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public bool Data { get; set; }
    }
}
