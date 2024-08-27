
### Overview
This project provides an implementation for fetching and processing exchange rates from the Czech National Bank. The primary functionality is encapsulated within the ExchangeRateProvider directory, which contains several key components.

### File Descriptions
#### Module.cs
- This file contains the module configuration
#### ExchangeRateProvider Directory
- CzechNationalBankExchangeRateProvider.cs: Contains the main class responsible for retrieving and providing exchange rate data from the Czech National Bank.
- ExchangeRateHttpClient.cs: Manages HTTP communication for fetching exchange rates.
- ExchangeRateProviderSettings.cs: Stores and manages settings related to the exchange rate provider, such as API endpoints and configuration parameters.
- ExchangeRatesParallelHttpClient.cs: Handles parallel HTTP requests to fetch exchange rates, potentially to improve performance.
- IExchangeRateHttpClient.cs: Interface defining the contract for HTTP clients fetching exchange rates.
- IExchangeRatesParallelHttpClient.cs: Interface defining the contract for parallel HTTP clients fetching exchange rates.
- ProviderExchangeRate.cs: Represents the exchange rate data model used within the provider.