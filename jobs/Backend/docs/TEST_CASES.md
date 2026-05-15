# Test Cases

This file documents the current test coverage for the backend exchange-rate task.

## Test Files


| File                                                               | Scope                  | Purpose                                                                                                            |
| ------------------------------------------------------------------ | ---------------------- | ------------------------------------------------------------------------------------------------------------------ |
| `jobs/Backend/Task.Tests/ExchangeRateProviderTests.cs`             | Unit                   | Public `ExchangeRateProvider.GetExchangeRates` behavior with a fake `IExchangeRateSource`.                         |
| `jobs/Backend/Task.Tests/ExchangeRateProviderFilteringTests.cs`    | Unit                   | Private `ExchangeRateProvider.GetFilteredRates` behavior, invoked through reflection.                              |
| `jobs/Backend/Task.Tests/CnbExchangeRateSourceIntegrationTests.cs` | Integration-style unit | `CnbExchangeRateSource` HTTP response handling and CNB daily document parsing with a stubbed `HttpMessageHandler`. |


## Provider Unit Test Matrix

These tests exercise the public provider method:

`ExchangeRateProvider.GetExchangeRates(IEnumerable<Currency> currencies)`


| Test case                                           | Source rates                    | Requested currencies | Expected result                                                |
| --------------------------------------------------- | ------------------------------- | -------------------- | -------------------------------------------------------------- |
| Source returns no rates                             | None                            | `USD`                | Empty result.                                                  |
| One existing source currency requested              | `USD/CZK = 25`                  | `USD`                | Returns the `USD/CZK` rate.                                    |
| Requested currency has same code as source currency | `new Currency("USD")/CZK = 25`  | separate `USD`       | Returns the rate because `Currency` equality is based on code. |
| No rate matches request                             | `GBP/JPY = 150`                 | `USD`                | Empty result.                                                  |
| Only unknown requested currencies                   | `USD/CZK = 25`                  | `XYZ`                | Empty result.                                                  |
| Multiple source rates, one requested source         | `USD/CZK = 25`, `EUR/CZK = 24`  | `USD`                | Returns only `USD/CZK`.                                        |
| Multiple source currencies requested                | `USD/CZK`, `EUR/CZK`, `JPY/CZK` | `USD`, `EUR`         | Returns `USD/CZK` and `EUR/CZK`; excludes `JPY/CZK`.           |
| Mix of known and unknown requested currencies       | `USD/CZK = 25`                  | `USD`, `XYZ`         | Returns `USD/CZK`; ignores `XYZ`.                              |
| Source publishes single direction                   | `USD/CZK = 25`                  | `USD`                | Returns only `USD/CZK`; does not synthesize `CZK/USD`.         |
| Empty request                                       | `USD/CZK = 25`                  | None                 | Empty result.                                                  |


## Filtering Unit Test Matrix

These tests exercise the provider's private filtering method directly through reflection:

`ExchangeRateProvider.GetFilteredRates(IEnumerable<ExchangeRate> cnbRates, IEnumerable<Currency> currencies)`


| Test case                                     | Input rates                    | Requested currencies | Expected result                                           |
| --------------------------------------------- | ------------------------------ | -------------------- | --------------------------------------------------------- |
| Returns rates for requested source currencies | `USD/CZK = 25`, `EUR/CZK = 24` | `USD`                | Returns only `USD/CZK`.                                   |
| Ignores unknown requested currencies          | `USD/CZK = 25`                 | `XYZ`                | Empty result.                                             |
| Does not create inverse pairs                 | `USD/CZK = 25`                 | `USD`                | Returns only the source-provided `USD/CZK`; no `CZK/USD`. |


## CNB Source Integration Test Matrix

These tests exercise the CNB source method using controlled HTTP responses:

`CnbExchangeRateSource.GetExchangeRates()`

Filtering by requested currencies is intentionally not covered here; the source returns parsed rows and the provider filters them.


| Scenario                                     | HTTP body shape                                | Expected parsed rates                  |
| -------------------------------------------- | ---------------------------------------------- | -------------------------------------- |
| `single_row_amount_one_dot_decimal`          | CNB headers plus one USD row with dot decimal  | Parses one `USD/CZK` rate.             |
| `amount_100_normalises_to_per_unit`          | CNB headers plus one JPY row with amount `100` | Normalizes the JPY rate per one unit.  |
| `multiple_rows`                              | CNB headers plus USD and EUR rows              | `USD/CZK = 20.648`, `EUR/CZK = 24.305` |
| `comma_decimal_czech_style`                  | CNB headers plus one USD row with comma decimal | Parses one `USD/CZK` rate.            |
| `malformed_pipe_row_skipped_valid_rows_kept` | Malformed row, valid USD row, empty pipe row   | Keeps only `USD/CZK = 1`.              |
| `empty_body`                                 | Empty HTTP body                                | Empty result.                          |
| `headers_only_no_data_rows`                  | CNB headers without data rows                  | Empty result.                          |
| `whitespace_trimmed_in_fields`               | CNB headers plus fields padded with whitespace | `USD/CZK = 10`                         |


## CNB Source Error Handling Matrix

Method under test:

`CnbExchangeRateSource.GetExchangeRates()`


| Test case          | HTTP status     | Expected result                |
| ------------------ | --------------- | ------------------------------ |
| HTTP request fails | `404 Not Found` | Throws `HttpRequestException`. |
