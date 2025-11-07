using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using ConsoleTestApp.Config;
using ConsoleTestApp.Core;
using ConsoleTestApp.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleTestApp.Clients;

public class SoapApiClient : IApiClient
{
    private readonly string _soapUrl;
    private readonly string _hubUrl;
    private string _authToken = string.Empty;
    private HubConnection? _hubConnection;

    public string Protocol => "SOAP (XML over HTTP)";
    public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);
    public bool SupportsStreaming => true;

    public SoapApiClient(ApiEndpoints endpoints)
    {
        _soapUrl = endpoints.Soap;
        _hubUrl = $"{endpoints.Soap}/hubs/exchange-rates";
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760, // 10MB
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/AuthenticationService.svc");
            var factory = new ChannelFactory<IAuthenticationServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            var request = new SoapLoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await client.LoginAsync(request);

            factory.Close();
            stopwatch.Stop();

            if (response.Success && response.Data != null)
            {
                _authToken = response.Data.AccessToken;

                return new AuthenticationResult
                {
                    Success = true,
                    Token = response.Data.AccessToken,
                    Email = response.Data.Email,
                    Role = response.Data.Role,
                    ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(response.Data.ExpiresAt).DateTime
                };
            }

            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = response.Message ?? "Login failed"
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            };
        }
    }

    public Task LogoutAsync()
    {
        _authToken = string.Empty;
        return Task.CompletedTask;
    }

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRatesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.svc");
            var factory = new ChannelFactory<IExchangeRateServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            // Add JWT token to headers (WCF doesn't have built-in JWT support, so we use OperationContext)
            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetAllLatestRatesGroupedRequest();
                var response = await client.GetAllLatestRatesGroupedAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = MapToExchangeRateData(response.Data);

                    // Estimate payload size (SOAP is XML)
                    var payloadSize = EstimateXmlSize(response.Data);

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
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetHistoricalRatesAsync(DateTime from, DateTime to)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.svc");
            var factory = new ChannelFactory<IExchangeRateServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetAllLatestRatesGroupedRequest();
                var response = await client.GetHistoricalRatesUpdateAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = MapToExchangeRateData(response.Data);
                    var payloadSize = EstimateXmlSize(response.Data);

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
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
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

        // SignalR sends XML strings (SOAP envelope) - we need to deserialize
        _hubConnection.On<string>("LatestRatesUpdated", (xmlData) =>
        {
            try
            {
                var data = DeserializeXmlData(xmlData);
                if (data != null)
                {
                    onUpdate(MapToExchangeRateData(data));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing XML update: {ex.Message}");
            }
        });

        _hubConnection.On<string>("HistoricalRatesUpdated", (xmlData) =>
        {
            try
            {
                var data = DeserializeXmlData(xmlData);
                if (data != null)
                {
                    onUpdate(MapToExchangeRateData(data));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing XML update: {ex.Message}");
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

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.svc");
            var factory = new ChannelFactory<IExchangeRateServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetCurrentRatesRequest();
                var response = await client.GetCurrentRatesAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = MapToExchangeRateData(response.Data);
                    var payloadSize = EstimateXmlSize(response.Data);

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
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ExchangeRateData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(ConversionResult Data, ApiCallMetrics Metrics)> ConvertCurrencyAsync(string from, string to, decimal amount)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.svc");
            var factory = new ChannelFactory<IExchangeRateServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapConvertCurrencyRequest
                {
                    FromCurrency = from,
                    ToCurrency = to,
                    Amount = amount
                };
                var response = await client.ConvertCurrencyAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ConversionResult
                    {
                        FromCurrency = response.Data.SourceCurrencyCode,
                        ToCurrency = response.Data.TargetCurrencyCode,
                        Amount = response.Data.SourceAmount,
                        ConvertedAmount = response.Data.TargetAmount,
                        Rate = response.Data.EffectiveRate,
                        ValidDate = DateTime.Parse(response.Data.ValidDate)
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = Encoding.UTF8.GetByteCount(response.Message ?? ""),
                        Success = true
                    });
                }

                return (new ConversionResult(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ConversionResult(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(CurrenciesListData Data, ApiCallMetrics Metrics)> GetCurrenciesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/CurrencyService.svc");
            var factory = new ChannelFactory<ICurrencyServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetAllCurrenciesRequest();
                var response = await client.GetAllCurrenciesAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new CurrenciesListData
                    {
                        Currencies = response.Data.Select(c => new CurrencyData
                        {
                            Id = c.Id,
                            Code = c.Code,
                            Name = c.Name,
                            Symbol = c.Symbol,
                            DecimalPlaces = c.DecimalPlaces,
                            IsActive = c.IsActive
                        }).ToList()
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = response.Data.Length * 100, // Estimate
                        Success = true
                    });
                }

                return (new CurrenciesListData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new CurrenciesListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/CurrencyService.svc");
            var factory = new ChannelFactory<ICurrencyServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetCurrencyByCodeRequest { Code = code };
                var response = await client.GetCurrencyByCodeAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new CurrencyData
                    {
                        Id = response.Data.Id,
                        Code = response.Data.Code,
                        Name = response.Data.Name,
                        Symbol = response.Data.Symbol,
                        DecimalPlaces = response.Data.DecimalPlaces,
                        IsActive = response.Data.IsActive
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 100, // Estimate
                        Success = true
                    });
                }

                return (new CurrencyData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new CurrencyData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(ProvidersListData Data, ApiCallMetrics Metrics)> GetProvidersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.svc");
            var factory = new ChannelFactory<IProviderServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetAllProvidersRequest();
                var response = await client.GetAllProvidersAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ProvidersListData
                    {
                        Providers = response.Data.Select(p => new ProviderData
                        {
                            Code = p.Code,
                            Name = p.Name,
                            BaseUrl = p.BaseUrl,
                            IsActive = p.IsActive
                        }).ToList()
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = response.Data.Length * 150, // Estimate
                        Success = true
                    });
                }

                return (new ProvidersListData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProvidersListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.svc");
            var factory = new ChannelFactory<IProviderServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetProviderByCodeRequest { Code = code };
                var response = await client.GetProviderByCodeAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ProviderData
                    {
                        Code = response.Data.Code,
                        Name = response.Data.Name,
                        BaseUrl = response.Data.BaseUrl,
                        IsActive = response.Data.IsActive
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 150, // Estimate
                        Success = true
                    });
                }

                return (new ProviderData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(ProviderHealthData Data, ApiCallMetrics Metrics)> GetProviderHealthAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.svc");
            var factory = new ChannelFactory<IProviderServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetProviderHealthRequest { Code = code };
                var response = await client.GetProviderHealthAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ProviderHealthData
                    {
                        ProviderCode = response.Data.ProviderCode,
                        ProviderName = response.Data.ProviderName,
                        IsHealthy = response.Data.IsHealthy,
                        ConsecutiveFailures = response.Data.ConsecutiveFailures,
                        LastSuccessfulFetch = string.IsNullOrEmpty(response.Data.LastSuccessfulFetch)
                            ? null
                            : DateTime.Parse(response.Data.LastSuccessfulFetch),
                        LastFailedFetch = string.IsNullOrEmpty(response.Data.LastFailedFetch)
                            ? null
                            : DateTime.Parse(response.Data.LastFailedFetch),
                        LastError = response.Data.LastError
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 200, // Estimate
                        Success = true
                    });
                }

                return (new ProviderHealthData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderHealthData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(ProviderStatisticsData Data, ApiCallMetrics Metrics)> GetProviderStatisticsAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.svc");
            var factory = new ChannelFactory<IProviderServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetProviderStatisticsRequest { Code = code };
                var response = await client.GetProviderStatisticsAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ProviderStatisticsData
                    {
                        ProviderCode = response.Data.ProviderCode,
                        TotalFetches = response.Data.TotalFetches,
                        SuccessfulFetches = response.Data.SuccessfulFetches,
                        SuccessRate = response.Data.SuccessRate,
                        TotalRatesProvided = response.Data.TotalRatesProvided
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 150, // Estimate
                        Success = true
                    });
                }

                return (new ProviderStatisticsData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderStatisticsData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.svc");
            var factory = new ChannelFactory<IUserServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetAllUsersRequest();
                var response = await client.GetAllUsersAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new UsersListData
                    {
                        Users = response.Data.Select(u => new UserData
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Role = u.Role,
                            IsActive = u.IsActive,
                            CreatedAt = DateTime.Parse(u.CreatedAt)
                        }).ToList()
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = response.Data.Length * 100, // Estimate
                        Success = true
                    });
                }

                return (new UsersListData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new UsersListData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<(UserData Data, ApiCallMetrics Metrics)> GetUserAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.svc");
            var factory = new ChannelFactory<IUserServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetUserByIdRequest { Id = id };
                var response = await client.GetUserByIdAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new UserData
                    {
                        Id = response.Data.Id,
                        Email = response.Data.Email,
                        Role = response.Data.Role,
                        IsActive = response.Data.IsActive,
                        CreatedAt = DateTime.Parse(response.Data.CreatedAt)
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 100, // Estimate
                        Success = true
                    });
                }

                return (new UserData(), new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = response.Message ?? "Request failed"
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new UserData(), new ApiCallMetrics
            {
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                Success = false,
                ErrorMessage = $"SOAP Exception: {ex.Message}"
            });
        }
    }

    public async Task<bool> IsApiAvailableAsync()
    {
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None },
                OpenTimeout = TimeSpan.FromSeconds(5),
                SendTimeout = TimeSpan.FromSeconds(5),
                ReceiveTimeout = TimeSpan.FromSeconds(5)
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.svc");
            var factory = new ChannelFactory<IExchangeRateServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            var request = new SoapGetAllLatestRatesGroupedRequest();
            await client.GetAllLatestRatesGroupedAsync(request);

            factory.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static SoapLatestExchangeRatesGrouped[]? DeserializeXmlData(string xmlData)
    {
        try
        {
            var serializer = new DataContractSerializer(typeof(SoapLatestExchangeRatesGrouped[]));

            using var stringReader = new StringReader(xmlData);
            using var xmlReader = XmlReader.Create(stringReader);

            // Navigate to the data inside SOAP envelope
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element &&
                    xmlReader.LocalName == "ArrayOfLatestExchangeRatesGroupedSoap")
                {
                    return (SoapLatestExchangeRatesGrouped[]?)serializer.ReadObject(xmlReader);
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static ExchangeRateData MapToExchangeRateData(SoapLatestExchangeRatesGrouped[] soapData)
    {
        var data = new ExchangeRateData
        {
            FetchedAt = DateTime.UtcNow,
            Providers = new List<ProviderRates>()
        };

        foreach (var provider in soapData)
        {
            var providerRates = new ProviderRates
            {
                ProviderCode = provider.Provider.Code,
                ProviderName = provider.Provider.Name,
                BaseCurrencies = new List<BaseCurrencyRates>()
            };

            foreach (var baseCurrency in provider.BaseCurrencies)
            {
                var baseCurrencyRates = new BaseCurrencyRates
                {
                    CurrencyCode = baseCurrency.BaseCurrency,
                    TargetRates = new List<TargetRate>()
                };

                foreach (var rate in baseCurrency.Rates)
                {
                    baseCurrencyRates.TargetRates.Add(new TargetRate
                    {
                        CurrencyCode = rate.TargetCurrency,
                        Rate = rate.RateInfo.Rate,
                        Multiplier = rate.RateInfo.Multiplier,
                        ValidDate = DateTime.Parse(rate.ValidDate)
                    });
                }

                providerRates.BaseCurrencies.Add(baseCurrencyRates);
                data.TotalRates += baseCurrencyRates.TargetRates.Count;
            }

            data.Providers.Add(providerRates);
        }

        return data;
    }

    private static int EstimateXmlSize(SoapLatestExchangeRatesGrouped[] data)
    {
        // Rough estimate: each provider ~500 bytes + each rate ~150 bytes
        int size = 0;
        foreach (var provider in data)
        {
            size += 500; // Provider overhead
            foreach (var baseCurrency in provider.BaseCurrencies)
            {
                size += baseCurrency.Rates.Length * 150;
            }
        }
        return size;
    }

    // ============================================================
    // SOAP SERVICE CONTRACTS
    // ============================================================

    [ServiceContract]
    private interface IAuthenticationServiceSoap
    {
        [OperationContract]
        Task<SoapLoginResponse> LoginAsync(SoapLoginRequest request);
    }

    [ServiceContract]
    private interface IExchangeRateServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllLatestRatesGroupedResponse> GetAllLatestRatesGroupedAsync(SoapGetAllLatestRatesGroupedRequest request);

        [OperationContract]
        Task<SoapGetAllLatestRatesGroupedResponse> GetHistoricalRatesUpdateAsync(SoapGetAllLatestRatesGroupedRequest request);

        [OperationContract]
        Task<SoapGetAllLatestRatesGroupedResponse> GetCurrentRatesAsync(SoapGetCurrentRatesRequest request);

        [OperationContract]
        Task<SoapConvertCurrencyResponse> ConvertCurrencyAsync(SoapConvertCurrencyRequest request);
    }

    [ServiceContract]
    private interface ICurrencyServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllCurrenciesResponse> GetAllCurrenciesAsync(SoapGetAllCurrenciesRequest request);

        [OperationContract]
        Task<SoapGetCurrencyByCodeResponse> GetCurrencyByCodeAsync(SoapGetCurrencyByCodeRequest request);
    }

    [ServiceContract]
    private interface IProviderServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllProvidersResponse> GetAllProvidersAsync(SoapGetAllProvidersRequest request);

        [OperationContract]
        Task<SoapGetProviderByCodeResponse> GetProviderByCodeAsync(SoapGetProviderByCodeRequest request);

        [OperationContract]
        Task<SoapGetProviderHealthResponse> GetProviderHealthAsync(SoapGetProviderHealthRequest request);

        [OperationContract]
        Task<SoapGetProviderStatisticsResponse> GetProviderStatisticsAsync(SoapGetProviderStatisticsRequest request);
    }

    [ServiceContract]
    private interface IUserServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllUsersResponse> GetAllUsersAsync(SoapGetAllUsersRequest request);

        [OperationContract]
        Task<SoapGetUserByIdResponse> GetUserByIdAsync(SoapGetUserByIdRequest request);
    }

    // ============================================================
    // SOAP DATA CONTRACTS
    // ============================================================

    [DataContract]
    private class SoapLoginRequest
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Password { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapLoginResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapAuthenticationData? Data { get; set; }
    }

    [DataContract]
    private class SoapAuthenticationData
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Role { get; set; } = string.Empty;

        [DataMember]
        public string AccessToken { get; set; } = string.Empty;

        [DataMember]
        public long ExpiresAt { get; set; }
    }

    [DataContract]
    private class SoapGetAllLatestRatesGroupedRequest
    {
        // Empty request
    }

    [DataContract]
    private class SoapGetAllLatestRatesGroupedResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapLatestExchangeRatesGrouped[] Data { get; set; } = Array.Empty<SoapLatestExchangeRatesGrouped>();
    }

    [DataContract]
    private class SoapLatestExchangeRatesGrouped
    {
        [DataMember]
        public SoapProviderInfo Provider { get; set; } = new();

        [DataMember]
        public SoapBaseCurrencyGroup[] BaseCurrencies { get; set; } = Array.Empty<SoapBaseCurrencyGroup>();
    }

    [DataContract]
    private class SoapProviderInfo
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapBaseCurrencyGroup
    {
        [DataMember]
        public string BaseCurrency { get; set; } = string.Empty;

        [DataMember]
        public SoapTargetCurrencyRate[] Rates { get; set; } = Array.Empty<SoapTargetCurrencyRate>();
    }

    [DataContract]
    private class SoapTargetCurrencyRate
    {
        [DataMember]
        public string TargetCurrency { get; set; } = string.Empty;

        [DataMember]
        public SoapRateInfo RateInfo { get; set; } = new();

        [DataMember]
        public string ValidDate { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapRateInfo
    {
        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public int Multiplier { get; set; }
    }

    // New Data Contracts for additional endpoints

    [DataContract]
    private class SoapGetCurrentRatesRequest
    {
        // Empty request
    }

    [DataContract]
    private class SoapConvertCurrencyRequest
    {
        [DataMember]
        public string FromCurrency { get; set; } = string.Empty;

        [DataMember]
        public string ToCurrency { get; set; } = string.Empty;

        [DataMember]
        public decimal Amount { get; set; }
    }

    [DataContract]
    private class SoapConvertCurrencyResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapConversionData? Data { get; set; }
    }

    [DataContract]
    private class SoapConversionData
    {
        [DataMember]
        public string SourceCurrencyCode { get; set; } = string.Empty;

        [DataMember]
        public string TargetCurrencyCode { get; set; } = string.Empty;

        [DataMember]
        public decimal SourceAmount { get; set; }

        [DataMember]
        public decimal TargetAmount { get; set; }

        [DataMember]
        public decimal EffectiveRate { get; set; }

        [DataMember]
        public string ValidDate { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapGetAllCurrenciesRequest
    {
        // Empty request
    }

    [DataContract]
    private class SoapGetAllCurrenciesResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapCurrencyData[] Data { get; set; } = Array.Empty<SoapCurrencyData>();
    }

    [DataContract]
    private class SoapGetCurrencyByCodeRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapGetCurrencyByCodeResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapCurrencyData? Data { get; set; }
    }

    [DataContract]
    private class SoapCurrencyData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string? Symbol { get; set; }

        [DataMember]
        public int DecimalPlaces { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }

    [DataContract]
    private class SoapGetAllProvidersRequest
    {
        // Empty request
    }

    [DataContract]
    private class SoapGetAllProvidersResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderData[] Data { get; set; } = Array.Empty<SoapProviderData>();
    }

    [DataContract]
    private class SoapGetProviderByCodeRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapGetProviderByCodeResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderData? Data { get; set; }
    }

    [DataContract]
    private class SoapProviderData
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string BaseUrl { get; set; } = string.Empty;

        [DataMember]
        public bool IsActive { get; set; }
    }

    [DataContract]
    private class SoapGetProviderHealthRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapGetProviderHealthResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderHealthData? Data { get; set; }
    }

    [DataContract]
    private class SoapProviderHealthData
    {
        [DataMember]
        public string ProviderCode { get; set; } = string.Empty;

        [DataMember]
        public string ProviderName { get; set; } = string.Empty;

        [DataMember]
        public bool IsHealthy { get; set; }

        [DataMember]
        public int ConsecutiveFailures { get; set; }

        [DataMember]
        public string? LastSuccessfulFetch { get; set; }

        [DataMember]
        public string? LastFailedFetch { get; set; }

        [DataMember]
        public string? LastError { get; set; }
    }

    [DataContract]
    private class SoapGetProviderStatisticsRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract]
    private class SoapGetProviderStatisticsResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderStatisticsData? Data { get; set; }
    }

    [DataContract]
    private class SoapProviderStatisticsData
    {
        [DataMember]
        public string ProviderCode { get; set; } = string.Empty;

        [DataMember]
        public int TotalFetches { get; set; }

        [DataMember]
        public int SuccessfulFetches { get; set; }

        [DataMember]
        public double SuccessRate { get; set; }

        [DataMember]
        public int TotalRatesProvided { get; set; }
    }

    [DataContract]
    private class SoapGetAllUsersRequest
    {
        // Empty request
    }

    [DataContract]
    private class SoapGetAllUsersResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapUserData[] Data { get; set; } = Array.Empty<SoapUserData>();
    }

    [DataContract]
    private class SoapGetUserByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    private class SoapGetUserByIdResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapUserData? Data { get; set; }
    }

    [DataContract]
    private class SoapUserData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Role { get; set; } = string.Empty;

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string CreatedAt { get; set; } = string.Empty;
    }
}
