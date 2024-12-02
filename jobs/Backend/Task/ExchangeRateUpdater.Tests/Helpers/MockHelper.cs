using ExchangeRates.Core.Models.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRates.Tests.Helpers
{
    internal static class MockHelper
    {
        /// <summary>
        /// Returns a mock HttpClient that returns a successful response with exchange rates.
        /// </summary>
        internal static HttpClient GetMockClientThatReturnsSuccessfulResponse()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"
                {
                    ""rates"": [
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Austrálie"",
                            ""currency"": ""dolar"",
                            ""amount"": 1,
                            ""currencyCode"": ""AUD"",
                            ""rate"": 15.58
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Brazílie"",
                            ""currency"": ""real"",
                            ""amount"": 1,
                            ""currencyCode"": ""BRL"",
                            ""rate"": 3.983
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Bulharsko"",
                            ""currency"": ""lev"",
                            ""amount"": 1,
                            ""currencyCode"": ""BGN"",
                            ""rate"": 12.921
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Čína"",
                            ""currency"": ""žen-min-pi"",
                            ""amount"": 1,
                            ""currencyCode"": ""CNY"",
                            ""rate"": 3.307
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Dánsko"",
                            ""currency"": ""koruna"",
                            ""amount"": 1,
                            ""currencyCode"": ""DKK"",
                            ""rate"": 3.388
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""EMU"",
                            ""currency"": ""euro"",
                            ""amount"": 1,
                            ""currencyCode"": ""EUR"",
                            ""rate"": 25.27
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Filipíny"",
                            ""currency"": ""peso"",
                            ""amount"": 100,
                            ""currencyCode"": ""PHP"",
                            ""rate"": 40.991
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Hongkong"",
                            ""currency"": ""dolar"",
                            ""amount"": 1,
                            ""currencyCode"": ""HKD"",
                            ""rate"": 3.091
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Indie"",
                            ""currency"": ""rupie"",
                            ""amount"": 100,
                            ""currencyCode"": ""INR"",
                            ""rate"": 28.391
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Indonesie"",
                            ""currency"": ""rupie"",
                            ""amount"": 1000,
                            ""currencyCode"": ""IDR"",
                            ""rate"": 1.512
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Island"",
                            ""currency"": ""koruna"",
                            ""amount"": 100,
                            ""currencyCode"": ""ISK"",
                            ""rate"": 17.32
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Izrael"",
                            ""currency"": ""nový šekel"",
                            ""amount"": 1,
                            ""currencyCode"": ""ILS"",
                            ""rate"": 6.614
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Japonsko"",
                            ""currency"": ""jen"",
                            ""amount"": 100,
                            ""currencyCode"": ""JPY"",
                            ""rate"": 16.022
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Jižní Afrika"",
                            ""currency"": ""rand"",
                            ""amount"": 1,
                            ""currencyCode"": ""ZAR"",
                            ""rate"": 1.323
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Kanada"",
                            ""currency"": ""dolar"",
                            ""amount"": 1,
                            ""currencyCode"": ""CAD"",
                            ""rate"": 17.127
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Korejská republika"",
                            ""currency"": ""won"",
                            ""amount"": 100,
                            ""currencyCode"": ""KRW"",
                            ""rate"": 1.712
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Maďarsko"",
                            ""currency"": ""forint"",
                            ""amount"": 100,
                            ""currencyCode"": ""HUF"",
                            ""rate"": 6.089
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Malajsie"",
                            ""currency"": ""ringgit"",
                            ""amount"": 1,
                            ""currencyCode"": ""MYR"",
                            ""rate"": 5.392
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Mexiko"",
                            ""currency"": ""peso"",
                            ""amount"": 1,
                            ""currencyCode"": ""MXN"",
                            ""rate"": 1.172
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""MMF"",
                            ""currency"": ""ZPČ"",
                            ""amount"": 1,
                            ""currencyCode"": ""XDR"",
                            ""rate"": 31.597
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Norsko"",
                            ""currency"": ""koruna"",
                            ""amount"": 1,
                            ""currencyCode"": ""NOK"",
                            ""rate"": 2.167
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Nový Zéland"",
                            ""currency"": ""dolar"",
                            ""amount"": 1,
                            ""currencyCode"": ""NZD"",
                            ""rate"": 14.158
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Polsko"",
                            ""currency"": ""zlotý"",
                            ""amount"": 1,
                            ""currencyCode"": ""PLN"",
                            ""rate"": 5.887
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Rumunsko"",
                            ""currency"": ""leu"",
                            ""amount"": 1,
                            ""currencyCode"": ""RON"",
                            ""rate"": 5.076
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Singapur"",
                            ""currency"": ""dolar"",
                            ""amount"": 1,
                            ""currencyCode"": ""SGD"",
                            ""rate"": 17.872
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Švédsko"",
                            ""currency"": ""koruna"",
                            ""amount"": 1,
                            ""currencyCode"": ""SEK"",
                            ""rate"": 2.192
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Švýcarsko"",
                            ""currency"": ""frank"",
                            ""amount"": 1,
                            ""currencyCode"": ""CHF"",
                            ""rate"": 27.126
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Thajsko"",
                            ""currency"": ""baht"",
                            ""amount"": 100,
                            ""currencyCode"": ""THB"",
                            ""rate"": 69.795
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Turecko"",
                            ""currency"": ""lira"",
                            ""amount"": 100,
                            ""currencyCode"": ""TRY"",
                            ""rate"": 69.278
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""USA"",
                            ""currency"": ""dolar"",
                            ""amount"": 1,
                            ""currencyCode"": ""USD"",
                            ""rate"": 24.048
                        },
                        {
                            ""validFor"": ""2024-12-02"",
                            ""order"": 234,
                            ""country"": ""Velká Británie"",
                            ""currency"": ""libra"",
                            ""amount"": 1,
                            ""currencyCode"": ""GBP"",
                            ""rate"": 30.475
                        }
                    ]
                }")
                });
            return new HttpClient(mockHttpMessageHandler.Object);
        }

        /// <summary>
        /// Returns a mock HttpClient that returns a 500 error response.
        /// </summary>
        internal static HttpClient GetMockClientThatReturnsErrorResponse()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });
            return new HttpClient(mockHttpMessageHandler.Object);
        }

        /// <summary>
        /// Returns a mock of configuration settings.
        /// </summary>
        internal static IOptions<ExchangeRateSettings> GetMockConfigurationSettings()
        {
            return Options.Create(new ExchangeRateSettings
            {
                CnbApiUrl = "http://fakeurl.com",
                BaseCurrency = "CZK",
                CacheDurationInMinutes = 60
            });
        }

        /// <summary>
        /// Returns a mock of IMemoryCache that always returns false when trying to get a value from the cache.
        /// </summary>
        /// <returns></returns>
        internal static IMemoryCache GetMemoryCacheMock()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();

            // Set up the memory cache mock
            object? cacheEntry = null;
            memoryCacheMock
                .Setup(m => m.TryGetValue(It.IsAny<object>(), out cacheEntry))
                .Returns(false);

            memoryCacheMock
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>());

            return memoryCacheMock.Object;
        }
    }
}