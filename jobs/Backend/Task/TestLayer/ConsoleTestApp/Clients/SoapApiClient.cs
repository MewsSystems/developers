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

            var endpoint = new EndpointAddress($"{_soapUrl}/AuthenticationService.asmx");
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.asmx");
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.asmx");
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.asmx");
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

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetCurrentRatesGroupedAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.asmx");
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

    public async Task<(ExchangeRateData Data, ApiCallMetrics Metrics)> GetLatestRateAsync(string sourceCurrency, string targetCurrency, int? providerId = null)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.asmx");
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

                var request = new SoapGetLatestRateRequest
                {
                    SourceCurrency = sourceCurrency,
                    TargetCurrency = targetCurrency,
                    ProviderId = providerId
                };
                var response = await client.GetLatestRateAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ExchangeRateData
                    {
                        Providers = new List<ProviderRates>
                        {
                            new ProviderRates
                            {
                                ProviderCode = response.Data.ProviderCode ?? "Unknown",
                                ProviderName = response.Data.ProviderCode ?? "Unknown",
                                BaseCurrencies = new List<BaseCurrencyRates>
                                {
                                    new BaseCurrencyRates
                                    {
                                        CurrencyCode = sourceCurrency,
                                        TargetRates = new List<TargetRate>
                                        {
                                            new TargetRate
                                            {
                                                CurrencyCode = targetCurrency,
                                                Rate = response.Data.Rate,
                                                ValidDate = DateTime.TryParse(response.Data.Timestamp, out var ts) ? ts : DateTime.UtcNow
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 200, // Estimate
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ExchangeRateService.asmx");
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

            var endpoint = new EndpointAddress($"{_soapUrl}/CurrencyService.asmx");
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
                            Name = c.Code, // Use code as name since server doesn't provide name
                            Symbol = null,
                            DecimalPlaces = 2, // Default
                            IsActive = true // Default
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

    public async Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/CurrencyService.asmx");
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

                var request = new SoapGetCurrencyByIdRequest { Id = id };
                var response = await client.GetCurrencyByIdAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new CurrencyData
                    {
                        Id = response.Data.Id,
                        Code = response.Data.Code,
                        Name = response.Data.Code, // Use code as name since server doesn't provide name
                        Symbol = null,
                        DecimalPlaces = 2, // Default
                        IsActive = true // Default
                    };

                    return (data, new ApiCallMetrics
                    {
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        PayloadSizeBytes = 150, // Estimate
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

    public async Task<(CurrencyData Data, ApiCallMetrics Metrics)> GetCurrencyByCodeAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/CurrencyService.asmx");
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
                        Name = response.Data.Code, // Use code as name since server doesn't provide name
                        Symbol = null,
                        DecimalPlaces = 2, // Default
                        IsActive = true // Default
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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
                            BaseUrl = p.Url,
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

    public async Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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

                var request = new SoapGetProviderByIdRequest { Id = id };
                var response = await client.GetProviderByIdAsync(request);

                factory.Close();
                stopwatch.Stop();

                if (response.Success && response.Data != null)
                {
                    var data = new ProviderData
                    {
                        Id = response.Data.Id,
                        Code = response.Data.Code,
                        Name = response.Data.Name,
                        Description = response.Data.Description,
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
                ErrorMessage = ex.Message
            });
        }
    }

    public async Task<(ProviderData Data, ApiCallMetrics Metrics)> GetProviderByCodeAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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
                        BaseUrl = response.Data.Url,
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> RescheduleProviderAsync(string code, string updateTime, string timeZone)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 10485760,
                Security = { Mode = BasicHttpSecurityMode.None }
            };

            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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

                var request = new SoapRescheduleProviderRequest
                {
                    ProviderCode = code,
                    UpdateTime = updateTime,
                    TimeZone = timeZone
                };
                var response = await client.RescheduleProviderAsync(request);

                factory.Close();
                stopwatch.Stop();

                var data = new OperationResult
                {
                    Success = response.Success,
                    Message = response.Message ?? string.Empty,
                    ErrorMessage = response.Success ? string.Empty : (response.Message ?? "Request failed")
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    PayloadSizeBytes = 100, // Estimate
                    Success = response.Success
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new OperationResult { Success = false, ErrorMessage = $"SOAP Exception: {ex.Message}" },
                new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = false,
                    ErrorMessage = $"SOAP Exception: {ex.Message}"
                });
        }
    }

    // Provider management operations - Fully implemented in SOAP
    public async Task<(ProviderConfigurationData Data, ApiCallMetrics Metrics)> GetProviderConfigurationAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/ProviderService.asmx");
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

                var request = new SoapGetProviderConfigurationRequest { Code = code };
                var response = await client.GetProviderConfigurationAsync(request);
                factory.Close();
                stopwatch.Stop();

                var data = new ProviderConfigurationData
                {
                    Id = response.Data?.Id ?? 0,
                    Code = response.Data?.Code ?? "",
                    Name = response.Data?.Name ?? "",
                    Url = response.Data?.Url ?? "",
                    IsActive = response.Data?.IsActive ?? false,
                    BaseCurrencyCode = response.Data?.BaseCurrencyCode,
                    RequiresAuthentication = response.Data?.RequiresAuthentication ?? false,
                    ApiKeyVaultReference = response.Data?.ApiKeyVaultReference,
                    CreatedAt = string.IsNullOrEmpty(response.Data?.CreatedAt) ? DateTime.UtcNow : DateTime.Parse(response.Data.CreatedAt),
                    LastModifiedAt = string.IsNullOrEmpty(response.Data?.LastModifiedAt) ? null : DateTime.Parse(response.Data.LastModifiedAt)
                };

                return (data, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = response.Success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ProviderConfigurationData(), new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ActivateProviderAsync(string code)
    {
        return await ExecuteSoapProviderOperationAsync<SoapActivateProviderRequest, SoapActivateProviderResponse>(
            "ProviderService.asmx",
            new SoapActivateProviderRequest { Code = code },
            (client, request) => client.ActivateProviderAsync(request),
            "Provider activated successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeactivateProviderAsync(string code)
    {
        return await ExecuteSoapProviderOperationAsync<SoapDeactivateProviderRequest, SoapDeactivateProviderResponse>(
            "ProviderService.asmx",
            new SoapDeactivateProviderRequest { Code = code },
            (client, request) => client.DeactivateProviderAsync(request),
            "Provider deactivated successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ResetProviderHealthAsync(string code)
    {
        return await ExecuteSoapProviderOperationAsync<SoapResetProviderHealthRequest, SoapResetProviderHealthResponse>(
            "ProviderService.asmx",
            new SoapResetProviderHealthRequest { Code = code },
            (client, request) => client.ResetProviderHealthAsync(request),
            "Provider health reset successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> TriggerManualFetchAsync(string code)
    {
        return await ExecuteSoapProviderOperationAsync<SoapTriggerManualFetchRequest, SoapTriggerManualFetchResponse>(
            "ProviderService.asmx",
            new SoapTriggerManualFetchRequest { Code = code },
            (client, request) => client.TriggerManualFetchAsync(request),
            "Manual fetch triggered successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateProviderAsync(string name, string code, string url, int baseCurrencyId, bool requiresAuth, string? apiKeyRef)
    {
        return await ExecuteSoapProviderOperationAsync<SoapCreateProviderRequest, SoapCreateProviderResponse>(
            "ProviderService.asmx",
            new SoapCreateProviderRequest { Name = name, Code = code, Url = url, BaseCurrencyId = baseCurrencyId, RequiresAuthentication = requiresAuth, ApiKeyVaultReference = apiKeyRef },
            (client, request) => client.CreateProviderAsync(request),
            "Provider created successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> UpdateProviderConfigurationAsync(string code, string name, string url, bool requiresAuth, string? apiKeyRef)
    {
        return await ExecuteSoapProviderOperationAsync<SoapUpdateProviderConfigurationRequest, SoapUpdateProviderConfigurationResponse>(
            "ProviderService.asmx",
            new SoapUpdateProviderConfigurationRequest { Code = code, Name = name, Url = url, RequiresAuthentication = requiresAuth, ApiKeyVaultReference = apiKeyRef },
            (client, request) => client.UpdateProviderConfigurationAsync(request),
            "Provider configuration updated successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteProviderAsync(string code, bool force)
    {
        return await ExecuteSoapProviderOperationAsync<SoapDeleteProviderRequest, SoapDeleteProviderResponse>(
            "ProviderService.asmx",
            new SoapDeleteProviderRequest { Code = code, Force = force },
            (client, request) => client.DeleteProviderAsync(request),
            "Provider deleted successfully"
        );
    }

    private async Task<(OperationResult Data, ApiCallMetrics Metrics)> ExecuteSoapProviderOperationAsync<TRequest, TResponse>(
        string serviceName, TRequest request, Func<IProviderServiceSoap, TRequest, Task<TResponse>> operation, string successMessage)
        where TResponse : class, new()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/{serviceName}");
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

                var response = await operation(client, request);
                factory.Close();
                stopwatch.Stop();

                var successProp = response.GetType().GetProperty("Success");
                var messageProp = response.GetType().GetProperty("Message");
                var success = (bool)(successProp?.GetValue(response) ?? false);
                var message = (string?)(messageProp?.GetValue(response)) ?? successMessage;

                return (new OperationResult { Success = success, Message = message }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    // User management operations - Fully implemented in SOAP
    public async Task<(UserData Data, ApiCallMetrics Metrics)> GetUserByEmailAsync(string email)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.asmx");
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

                var request = new SoapGetUserByEmailRequest { Email = email };
                var response = await client.GetUserByEmailAsync(request);
                factory.Close();
                stopwatch.Stop();

                var data = new UserData
                {
                    Id = response.Data?.Id ?? 0,
                    Email = response.Data?.Email ?? "",
                    FirstName = response.Data?.FirstName ?? "",
                    LastName = response.Data?.LastName ?? "",
                    Role = response.Data?.Role ?? "",
                    IsActive = response.Data?.IsActive ?? false,
                    CreatedAt = string.IsNullOrEmpty(response.Data?.CreatedAt) ? DateTime.UtcNow : DateTime.Parse(response.Data.CreatedAt)
                };

                return (data, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = response.Success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new UserData(), new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    public async Task<(UsersListData Data, ApiCallMetrics Metrics)> GetUsersByRoleAsync(string role)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.asmx");
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

                var request = new SoapGetUsersByRoleRequest { Role = role };
                var response = await client.GetUsersByRoleAsync(request);
                factory.Close();
                stopwatch.Stop();

                var data = new UsersListData
                {
                    Users = response.Data?.Select(u => new UserData
                    {
                        Id = u.Id,
                        Email = u.Email ?? "",
                        FirstName = u.FirstName ?? "",
                        LastName = u.LastName ?? "",
                        Role = u.Role ?? "",
                        IsActive = u.IsActive,
                        CreatedAt = string.IsNullOrEmpty(u.CreatedAt) ? DateTime.UtcNow : DateTime.Parse(u.CreatedAt)
                    }).ToList() ?? new List<UserData>()
                };

                return (data, new ApiCallMetrics
                {
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    Success = response.Success,
                    ErrorMessage = response.Success ? null : (response.Fault?.Detail ?? response.Message ?? "Unknown error")
                });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new UsersListData(), new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CheckEmailExistsAsync(string email)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.asmx");
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

                var request = new SoapCheckEmailExistsRequest { Email = email };
                var response = await client.CheckEmailExistsAsync(request);
                factory.Close();
                stopwatch.Stop();

                return (new OperationResult
                {
                    Success = response.Exists,
                    Message = response.Message ?? (response.Exists ? "Email exists" : "Email does not exist")
                }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = response.Success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new OperationResult
            {
                Success = false,
                Message = ex.Message
            }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateUserAsync(string email, string password, string firstName, string lastName, string role)
    {
        return await ExecuteSoapUserOperationAsync<SoapCreateUserRequest, SoapCreateUserResponse>(
            new SoapCreateUserRequest { Email = email, Password = password, FirstName = firstName, LastName = lastName, Role = role },
            (client, request) => client.CreateUserAsync(request),
            "User created successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> UpdateUserAsync(int id, string firstName, string lastName)
    {
        return await ExecuteSoapUserOperationAsync<SoapUpdateUserRequest, SoapUpdateUserResponse>(
            new SoapUpdateUserRequest { UserId = id, FirstName = firstName, LastName = lastName, Email = "" },
            (client, request) => client.UpdateUserAsync(request),
            "User updated successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ChangePasswordAsync(int id, string currentPassword, string newPassword)
    {
        return await ExecuteSoapUserOperationAsync<SoapChangePasswordRequest, SoapChangePasswordResponse>(
            new SoapChangePasswordRequest { UserId = id, CurrentPassword = currentPassword, NewPassword = newPassword },
            (client, request) => client.ChangePasswordAsync(request),
            "Password changed successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> ChangeUserRoleAsync(int id, string newRole)
    {
        return await ExecuteSoapUserOperationAsync<SoapChangeUserRoleRequest, SoapChangeUserRoleResponse>(
            new SoapChangeUserRoleRequest { UserId = id, NewRole = newRole },
            (client, request) => client.ChangeUserRoleAsync(request),
            "User role changed successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteUserAsync(int id)
    {
        return await ExecuteSoapUserOperationAsync<SoapDeleteUserRequest, SoapDeleteUserResponse>(
            new SoapDeleteUserRequest { Id = id },
            (client, request) => client.DeleteUserAsync(request),
            "User deleted successfully"
        );
    }

    private async Task<(OperationResult Data, ApiCallMetrics Metrics)> ExecuteSoapUserOperationAsync<TRequest, TResponse>(
        TRequest request, Func<IUserServiceSoap, TRequest, Task<TResponse>> operation, string successMessage)
        where TResponse : class, new()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.asmx");
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

                var response = await operation(client, request);
                factory.Close();
                stopwatch.Stop();

                var successProp = response.GetType().GetProperty("Success");
                var messageProp = response.GetType().GetProperty("Message");
                var success = (bool)(successProp?.GetValue(response) ?? false);
                var message = (string?)(messageProp?.GetValue(response)) ?? successMessage;

                return (new OperationResult { Success = success, Message = message }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    // Currency management operations - Fully implemented in SOAP
    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> CreateCurrencyAsync(string code)
    {
        return await ExecuteSoapCurrencyOperationAsync<SoapCreateCurrencyRequest, SoapCreateCurrencyResponse>(
            new SoapCreateCurrencyRequest { Code = code },
            (client, request) => client.CreateCurrencyAsync(request),
            "Currency created successfully"
        );
    }

    public async Task<(OperationResult Data, ApiCallMetrics Metrics)> DeleteCurrencyAsync(string code)
    {
        return await ExecuteSoapCurrencyOperationAsync<SoapDeleteCurrencyRequest, SoapDeleteCurrencyResponse>(
            new SoapDeleteCurrencyRequest { Code = code },
            (client, request) => client.DeleteCurrencyAsync(request),
            "Currency deleted successfully"
        );
    }

    private async Task<(OperationResult Data, ApiCallMetrics Metrics)> ExecuteSoapCurrencyOperationAsync<TRequest, TResponse>(
        TRequest request, Func<ICurrencyServiceSoap, TRequest, Task<TResponse>> operation, string successMessage)
        where TResponse : class, new()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/CurrencyService.asmx");
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

                var response = await operation(client, request);
                factory.Close();
                stopwatch.Stop();

                var successProp = response.GetType().GetProperty("Success");
                var messageProp = response.GetType().GetProperty("Message");
                var success = (bool)(successProp?.GetValue(response) ?? false);
                var message = (string?)(messageProp?.GetValue(response)) ?? successMessage;

                return (new OperationResult { Success = success, Message = message }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new OperationResult { Success = false, ErrorMessage = ex.Message }, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    // System Health operations - Fully implemented in SOAP
    public async Task<(SystemHealthData Data, ApiCallMetrics Metrics)> GetSystemHealthAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/SystemHealthService.asmx");
            var factory = new ChannelFactory<ISystemHealthServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetSystemHealthRequest();
                var response = await client.GetSystemHealthAsync(request);
                factory.Close();
                stopwatch.Stop();

                var data = new SystemHealthData
                {
                    Status = response.Data?.Status ?? "",
                    TotalProviders = response.Data?.TotalProviders ?? 0,
                    HealthyProviders = response.Data?.HealthyProviders ?? 0,
                    UnhealthyProviders = response.Data?.UnhealthyProviders ?? 0,
                    TotalCurrencies = response.Data?.TotalCurrencies ?? 0,
                    TotalUsers = response.Data?.TotalUsers ?? 0,
                    TotalExchangeRates = response.Data?.TotalExchangeRates ?? 0
                };

                return (data, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = response.Success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new SystemHealthData(), new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    public async Task<(ErrorsListData Data, ApiCallMetrics Metrics)> GetRecentErrorsAsync(int count, string? severity)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/SystemHealthService.asmx");
            var factory = new ChannelFactory<ISystemHealthServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetRecentErrorsRequest { Count = count, Severity = severity };
                var response = await client.GetRecentErrorsAsync(request);
                factory.Close();
                stopwatch.Stop();

                var data = new ErrorsListData
                {
                    Errors = response.Data?.Select(e => new ErrorSummaryData
                    {
                        Id = e.Id,
                        ErrorMessage = e.ErrorMessage,
                        Severity = e.Severity,
                        SourceComponent = e.SourceComponent,
                        OccurredAt = string.IsNullOrEmpty(e.OccurredAt) ? DateTime.UtcNow : DateTime.Parse(e.OccurredAt),
                        ProviderId = e.ProviderId,
                        ProviderCode = e.ProviderCode
                    }).ToList() ?? new(),
                    TotalCount = response.Data?.Length ?? 0
                };

                return (data, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = response.Success });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new ErrorsListData(), new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
        }
    }

    public async Task<(FetchActivityListData Data, ApiCallMetrics Metrics)> GetFetchActivityAsync(int count, int? providerId, bool failedOnly)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var binding = new BasicHttpBinding { MaxReceivedMessageSize = 10485760, Security = { Mode = BasicHttpSecurityMode.None } };
            var endpoint = new EndpointAddress($"{_soapUrl}/SystemHealthService.asmx");
            var factory = new ChannelFactory<ISystemHealthServiceSoap>(binding, endpoint);
            var client = factory.CreateChannel();

            using (new OperationContextScope((IContextChannel)client))
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers["Authorization"] = $"Bearer {_authToken}";
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                }

                var request = new SoapGetFetchActivityRequest { Count = count, ProviderId = providerId, FailedOnly = failedOnly };
                var response = await client.GetFetchActivityAsync(request);
                factory.Close();
                stopwatch.Stop();

                var data = new FetchActivityListData
                {
                    Activities = response.Data?.Select(f => new FetchActivityData
                    {
                        Id = (int)f.LogId,
                        ProviderId = f.ProviderId,
                        ProviderCode = f.ProviderCode,
                        ProviderName = f.ProviderName,
                        FetchedAt = f.StartedAt.DateTime,
                        Success = f.Status.Equals("Success", StringComparison.OrdinalIgnoreCase),
                        RatesCount = (f.RatesImported ?? 0) + (f.RatesUpdated ?? 0),
                        ErrorMessage = f.ErrorMessage,
                        DurationMs = ParseDuration(f.Duration)
                    }).ToList() ?? new(),
                    TotalCount = response.Data?.Length ?? 0
                };

                return (data, new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = response.Success, PayloadSizeBytes = response.Data?.Length * 150 ?? 0 });
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return (new FetchActivityListData(), new ApiCallMetrics { ResponseTimeMs = stopwatch.ElapsedMilliseconds, Success = false, ErrorMessage = ex.Message });
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

            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.asmx");
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

            var endpoint = new EndpointAddress($"{_soapUrl}/UserService.asmx");
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
            // Just verify the SOAP endpoint URI is valid and accessible via HTTP
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            var response = await httpClient.GetAsync($"{_soapUrl}/ExchangeRateService.asmx");
            // SOAP service should return something (even if not 200 OK for GET)
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
                        Rate = rate.RateInfo.EffectiveRate,
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

    private static ExchangeRateData MapToExchangeRateData(SoapCurrentExchangeRatesGrouped[] soapData)
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
                        Rate = rate.RateInfo.EffectiveRate,
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

    private static int EstimateXmlSize(SoapCurrentExchangeRatesGrouped[] data)
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

    private static long ParseDuration(string? duration)
    {
        if (string.IsNullOrEmpty(duration))
            return 0;

        // Duration format from server is like "00:00:00.1234567" (TimeSpan format)
        if (TimeSpan.TryParse(duration, out var timeSpan))
        {
            return (long)timeSpan.TotalMilliseconds;
        }

        return 0;
    }

    // ============================================================
    // SOAP SERVICE CONTRACTS
    // ============================================================

    [ServiceContract(Namespace = "")]
    private interface IAuthenticationServiceSoap
    {
        [OperationContract]
        Task<SoapLoginResponse> LoginAsync(SoapLoginRequest request);
    }

    [ServiceContract(Namespace = "")]
    private interface IExchangeRateServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllLatestRatesGroupedResponse> GetAllLatestRatesGroupedAsync(SoapGetAllLatestRatesGroupedRequest request);

        [OperationContract]
        Task<SoapGetAllLatestRatesGroupedResponse> GetHistoricalRatesUpdateAsync(SoapGetAllLatestRatesGroupedRequest request);

        [OperationContract]
        Task<SoapGetCurrentRatesGroupedResponse> GetCurrentRatesAsync(SoapGetCurrentRatesRequest request);

        [OperationContract]
        Task<SoapGetLatestRateResponse> GetLatestRateAsync(SoapGetLatestRateRequest request);

        [OperationContract]
        Task<SoapConvertCurrencyResponse> ConvertCurrencyAsync(SoapConvertCurrencyRequest request);
    }

    [ServiceContract(Namespace = "")]
    private interface IProviderServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllProvidersResponse> GetAllProvidersAsync(SoapGetAllProvidersRequest request);

        [OperationContract]
        Task<SoapGetProviderByIdResponse> GetProviderByIdAsync(SoapGetProviderByIdRequest request);

        [OperationContract]
        Task<SoapGetProviderByCodeResponse> GetProviderByCodeAsync(SoapGetProviderByCodeRequest request);

        [OperationContract]
        Task<SoapGetProviderHealthResponse> GetProviderHealthAsync(SoapGetProviderHealthRequest request);

        [OperationContract]
        Task<SoapGetProviderStatisticsResponse> GetProviderStatisticsAsync(SoapGetProviderStatisticsRequest request);

        [OperationContract]
        Task<SoapGetProviderConfigurationResponse> GetProviderConfigurationAsync(SoapGetProviderConfigurationRequest request);

        [OperationContract]
        Task<SoapActivateProviderResponse> ActivateProviderAsync(SoapActivateProviderRequest request);

        [OperationContract]
        Task<SoapDeactivateProviderResponse> DeactivateProviderAsync(SoapDeactivateProviderRequest request);

        [OperationContract]
        Task<SoapResetProviderHealthResponse> ResetProviderHealthAsync(SoapResetProviderHealthRequest request);

        [OperationContract]
        Task<SoapTriggerManualFetchResponse> TriggerManualFetchAsync(SoapTriggerManualFetchRequest request);

        [OperationContract]
        Task<SoapCreateProviderResponse> CreateProviderAsync(SoapCreateProviderRequest request);

        [OperationContract]
        Task<SoapUpdateProviderConfigurationResponse> UpdateProviderConfigurationAsync(SoapUpdateProviderConfigurationRequest request);

        [OperationContract]
        Task<SoapDeleteProviderResponse> DeleteProviderAsync(SoapDeleteProviderRequest request);

        [OperationContract]
        Task<SoapRescheduleProviderResponse> RescheduleProviderAsync(SoapRescheduleProviderRequest request);
    }

    [ServiceContract(Namespace = "")]
    private interface IUserServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllUsersResponse> GetAllUsersAsync(SoapGetAllUsersRequest request);

        [OperationContract]
        Task<SoapGetUserByIdResponse> GetUserByIdAsync(SoapGetUserByIdRequest request);

        [OperationContract]
        Task<SoapGetUserByEmailResponse> GetUserByEmailAsync(SoapGetUserByEmailRequest request);

        [OperationContract]
        Task<SoapGetUsersByRoleResponse> GetUsersByRoleAsync(SoapGetUsersByRoleRequest request);

        [OperationContract]
        Task<SoapCheckEmailExistsResponse> CheckEmailExistsAsync(SoapCheckEmailExistsRequest request);

        [OperationContract]
        Task<SoapCreateUserResponse> CreateUserAsync(SoapCreateUserRequest request);

        [OperationContract]
        Task<SoapUpdateUserResponse> UpdateUserAsync(SoapUpdateUserRequest request);

        [OperationContract]
        Task<SoapChangePasswordResponse> ChangePasswordAsync(SoapChangePasswordRequest request);

        [OperationContract]
        Task<SoapChangeUserRoleResponse> ChangeUserRoleAsync(SoapChangeUserRoleRequest request);

        [OperationContract]
        Task<SoapDeleteUserResponse> DeleteUserAsync(SoapDeleteUserRequest request);
    }

    [ServiceContract(Namespace = "")]
    private interface ICurrencyServiceSoap
    {
        [OperationContract]
        Task<SoapGetAllCurrenciesResponse> GetAllCurrenciesAsync(SoapGetAllCurrenciesRequest request);

        [OperationContract]
        Task<SoapGetCurrencyByIdResponse> GetCurrencyByIdAsync(SoapGetCurrencyByIdRequest request);

        [OperationContract]
        Task<SoapGetCurrencyByCodeResponse> GetCurrencyByCodeAsync(SoapGetCurrencyByCodeRequest request);

        [OperationContract]
        Task<SoapCreateCurrencyResponse> CreateCurrencyAsync(SoapCreateCurrencyRequest request);

        [OperationContract]
        Task<SoapDeleteCurrencyResponse> DeleteCurrencyAsync(SoapDeleteCurrencyRequest request);
    }

    [ServiceContract(Namespace = "")]
    private interface ISystemHealthServiceSoap
    {
        [OperationContract]
        Task<SoapGetSystemHealthResponse> GetSystemHealthAsync(SoapGetSystemHealthRequest request);

        [OperationContract]
        Task<SoapGetRecentErrorsResponse> GetRecentErrorsAsync(SoapGetRecentErrorsRequest request);

        [OperationContract]
        Task<SoapGetFetchActivityResponse> GetFetchActivityAsync(SoapGetFetchActivityRequest request);
    }

    // ============================================================
    // SOAP DATA CONTRACTS
    // ============================================================

    [DataContract(Namespace = "")]
    private class SoapLoginRequest
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Password { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapLoginResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapAuthenticationData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
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

    [DataContract(Namespace = "")]
    private class SoapGetAllLatestRatesGroupedRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapGetAllLatestRatesGroupedResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapLatestExchangeRatesGrouped[] Data { get; set; } = Array.Empty<SoapLatestExchangeRatesGrouped>();
    }

    [DataContract(Namespace = "")]
    private class SoapLatestExchangeRatesGrouped
    {
        [DataMember]
        public SoapProviderInfo Provider { get; set; } = new();

        [DataMember]
        public SoapBaseCurrencyGroup[] BaseCurrencies { get; set; } = Array.Empty<SoapBaseCurrencyGroup>();
    }

    [DataContract(Namespace = "")]
    private class SoapProviderInfo
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapBaseCurrencyGroup
    {
        [DataMember]
        public string BaseCurrency { get; set; } = string.Empty;

        [DataMember]
        public SoapTargetCurrencyRate[] Rates { get; set; } = Array.Empty<SoapTargetCurrencyRate>();
    }

    [DataContract(Namespace = "")]
    private class SoapTargetCurrencyRate
    {
        [DataMember]
        public string TargetCurrency { get; set; } = string.Empty;

        [DataMember]
        public SoapRateInfo RateInfo { get; set; } = new();

        [DataMember]
        public string ValidDate { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapRateInfo
    {
        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public int Multiplier { get; set; }

        [DataMember]
        public decimal EffectiveRate { get; set; }
    }

    // New Data Contracts for additional endpoints

    [DataContract(Namespace = "")]
    private class SoapGetCurrentRatesRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapConvertCurrencyRequest
    {
        [DataMember]
        public string FromCurrency { get; set; } = string.Empty;

        [DataMember]
        public string ToCurrency { get; set; } = string.Empty;

        [DataMember]
        public decimal Amount { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapConvertCurrencyResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapConversionData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
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

    [DataContract(Namespace = "")]
    private class SoapGetAllCurrenciesRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapGetAllCurrenciesResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapCurrencyData[] Data { get; set; } = Array.Empty<SoapCurrencyData>();
    }

    [DataContract(Namespace = "")]
    private class SoapGetCurrencyByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetCurrencyByIdResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapCurrencyData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetCurrencyByCodeRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetCurrencyByCodeResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapCurrencyData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapCurrencyData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetAllProvidersRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapGetAllProvidersResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderData[] Data { get; set; } = Array.Empty<SoapProviderData>();
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderByCodeRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderByCodeResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapProviderData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Url { get; set; } = string.Empty;

        [DataMember]
        public string BaseCurrency { get; set; } = string.Empty;

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string HealthStatus { get; set; } = string.Empty;

        [DataMember]
        public int SuccessfulFetchCount { get; set; }

        [DataMember]
        public int FailedFetchCount { get; set; }

        [DataMember]
        public string? LastFetchAttempt { get; set; }

        [DataMember]
        public string? LastSuccessfulFetch { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderHealthRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderHealthResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderHealthData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
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

    [DataContract(Namespace = "")]
    private class SoapGetProviderStatisticsRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderStatisticsResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderStatisticsData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
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

    [DataContract(Namespace = "")]
    private class SoapRescheduleProviderRequest
    {
        [DataMember]
        public string ProviderCode { get; set; } = string.Empty;

        [DataMember]
        public string UpdateTime { get; set; } = string.Empty;

        [DataMember]
        public string TimeZone { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapRescheduleProviderResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetAllUsersRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapGetAllUsersResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapUserData[] Data { get; set; } = Array.Empty<SoapUserData>();
    }

    [DataContract(Namespace = "")]
    private class SoapGetUserByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetUserByIdResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapUserData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapUserData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public string FullName { get; set; } = string.Empty;

        [DataMember]
        public string Role { get; set; } = string.Empty;

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string CreatedAt { get; set; } = string.Empty;
    }

    // Provider Management Data Contracts
    [DataContract(Namespace = "")]
    private class SoapGetProviderConfigurationRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderConfigurationResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderDetailData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapProviderDetailData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string? Description { get; set; }

        [DataMember]
        public string Url { get; set; } = string.Empty;

        [DataMember]
        public string BaseUrl { get; set; } = string.Empty;

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string? BaseCurrencyCode { get; set; }

        [DataMember]
        public bool RequiresAuthentication { get; set; }

        [DataMember]
        public string? ApiKeyVaultReference { get; set; }

        [DataMember]
        public string CreatedAt { get; set; } = string.Empty;

        [DataMember]
        public string? LastModifiedAt { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapActivateProviderRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapActivateProviderResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapDeactivateProviderRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapDeactivateProviderResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapResetProviderHealthRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapResetProviderHealthResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapTriggerManualFetchRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapTriggerManualFetchResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapCreateProviderRequest
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Url { get; set; } = string.Empty;

        [DataMember]
        public int BaseCurrencyId { get; set; }

        [DataMember]
        public bool RequiresAuthentication { get; set; }

        [DataMember]
        public string? ApiKeyVaultReference { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapCreateProviderResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapUpdateProviderConfigurationRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Url { get; set; } = string.Empty;

        [DataMember]
        public bool RequiresAuthentication { get; set; }

        [DataMember]
        public string? ApiKeyVaultReference { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapUpdateProviderConfigurationResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapDeleteProviderRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;

        [DataMember]
        public bool Force { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapDeleteProviderResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    // User Management Data Contracts
    [DataContract(Namespace = "")]
    private class SoapGetUserByEmailRequest
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetUserByEmailResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapUserData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapCreateUserRequest
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Password { get; set; } = string.Empty;

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public string Role { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapCreateUserResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapUpdateUserRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapUpdateUserResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapChangePasswordRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string CurrentPassword { get; set; } = string.Empty;

        [DataMember]
        public string NewPassword { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapChangePasswordResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapFault? Fault { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapChangeUserRoleRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string NewRole { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapChangeUserRoleResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapDeleteUserRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapDeleteUserResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    // Currency Management Data Contracts
    [DataContract(Namespace = "")]
    private class SoapCreateCurrencyRequest
    {
        [DataMember]
        public string Code { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapCreateCurrencyResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapDeleteCurrencyRequest
    {
        [DataMember]
        public string Code { get; set; }= string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapDeleteCurrencyResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }

    // System Health Data Contracts
    [DataContract(Namespace = "")]
    private class SoapGetSystemHealthRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapGetSystemHealthResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapSystemHealthData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapSystemHealthData
    {
        [DataMember]
        public string Status { get; set; } = string.Empty;

        [DataMember]
        public int TotalProviders { get; set; }

        [DataMember]
        public int HealthyProviders { get; set; }

        [DataMember]
        public int UnhealthyProviders { get; set; }

        [DataMember]
        public int TotalCurrencies { get; set; }

        [DataMember]
        public int TotalUsers { get; set; }

        [DataMember]
        public long TotalExchangeRates { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetRecentErrorsRequest
    {
        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public string? Severity { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetRecentErrorsResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapErrorData[] Data { get; set; } = Array.Empty<SoapErrorData>();
    }

    [DataContract(Namespace = "")]
    private class SoapErrorData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember]
        public string? Severity { get; set; }

        [DataMember]
        public string? SourceComponent { get; set; }

        [DataMember]
        public string OccurredAt { get; set; } = string.Empty;

        [DataMember]
        public int? ProviderId { get; set; }

        [DataMember]
        public string? ProviderCode { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetFetchActivityRequest
    {
        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int? ProviderId { get; set; }

        [DataMember]
        public bool FailedOnly { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetFetchActivityResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapFetchActivityData[] Data { get; set; } = Array.Empty<SoapFetchActivityData>();
    }

    [DataContract(Namespace = "")]
    private class SoapFetchActivityData
    {
        [DataMember]
        public long LogId { get; set; }

        [DataMember]
        public int ProviderId { get; set; }

        [DataMember]
        public string ProviderCode { get; set; } = string.Empty;

        [DataMember]
        public string ProviderName { get; set; } = string.Empty;

        [DataMember]
        public string Status { get; set; } = string.Empty;

        [DataMember]
        public int? RatesImported { get; set; }

        [DataMember]
        public int? RatesUpdated { get; set; }

        [DataMember]
        public string? ErrorMessage { get; set; }

        [DataMember]
        public DateTimeOffset StartedAt { get; set; }

        [DataMember]
        public DateTimeOffset? CompletedAt { get; set; }

        [DataMember]
        public string? Duration { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderByIdRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetProviderByIdResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapProviderDetailData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetUsersByRoleRequest
    {
        [DataMember]
        public string Role { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapGetUsersByRoleResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapUserData[]? Data { get; set; }

        [DataMember]
        public SoapFault? Fault { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapCheckEmailExistsRequest
    {
        [DataMember]
        public string Email { get; set; } = string.Empty;
    }

    [DataContract(Namespace = "")]
    private class SoapCheckEmailExistsResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public bool Exists { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetCurrentRatesGroupedRequest
    {
        // Empty request
    }

    [DataContract(Namespace = "")]
    private class SoapGetCurrentRatesGroupedResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapCurrentExchangeRatesGrouped[]? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapCurrentExchangeRatesGrouped
    {
        [DataMember]
        public SoapProviderInfo Provider { get; set; } = new();

        [DataMember]
        public SoapCurrentBaseCurrencyGroup[] BaseCurrencies { get; set; } = Array.Empty<SoapCurrentBaseCurrencyGroup>();
    }

    [DataContract(Namespace = "")]
    private class SoapCurrentBaseCurrencyGroup
    {
        [DataMember]
        public string BaseCurrency { get; set; } = string.Empty;

        [DataMember]
        public SoapCurrentTargetCurrencyRate[] Rates { get; set; } = Array.Empty<SoapCurrentTargetCurrencyRate>();
    }

    [DataContract(Namespace = "")]
    private class SoapCurrentTargetCurrencyRate
    {
        [DataMember]
        public string TargetCurrency { get; set; } = string.Empty;

        [DataMember]
        public SoapRateInfo RateInfo { get; set; } = new();

        [DataMember]
        public string ValidDate { get; set; } = string.Empty;

        [DataMember]
        public string LastUpdated { get; set; } = string.Empty;

        [DataMember]
        public int DaysOld { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetLatestRateRequest
    {
        [DataMember]
        public string SourceCurrency { get; set; } = string.Empty;

        [DataMember]
        public string TargetCurrency { get; set; } = string.Empty;

        [DataMember]
        public int? ProviderId { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapGetLatestRateResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string? Message { get; set; }

        [DataMember]
        public SoapLatestRateData? Data { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapLatestRateData
    {
        [DataMember]
        public string? ProviderCode { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public string? Timestamp { get; set; }
    }

    [DataContract(Namespace = "")]
    private class SoapFault
    {
        [DataMember]
        public string FaultCode { get; set; } = string.Empty;

        [DataMember]
        public string FaultString { get; set; } = string.Empty;

        [DataMember]
        public string? Detail { get; set; }
    }
}
