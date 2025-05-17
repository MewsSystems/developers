# Exchange Rate Updater

## Overview

This project is a C# console application designed to fetch and update foreign exchange rates using data provided by the Czech National Bank (CNB). The application retrieves the latest daily exchange rates and can be integrated into financial systems or used as a standalone tool for currency conversion and rate analysis.

## Features
- Fetches up-to-date exchange rates from the official CNB source
- Parses and processes the daily exchange rate data
- Supports multiple currencies as provided by the CNB
- Modular and extensible codebase for easy integration

## Data Source
- **Provider:** Czech National Bank (CNB)
- **URL:** [CNB Daily Exchange Rates](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt)
- The data is updated daily by the CNB and includes rates for a wide range of currencies against the Czech Koruna (CZK).

## Usage
1. Build the project using the provided solution file (`ExchangeRateUpdater.sln`).
2. Run the application. It will automatically fetch the latest exchange rates from the CNB and process them.
3. The application can be extended to store rates in a database, expose them via an API, or integrate with other systems as needed.

## Project Structure
- `Currency.cs`: Defines currency-related data structures.
- `ExchangeRate.cs`: Represents exchange rate information.
- `ExchangeRateProvider.cs`: Handles fetching and parsing of exchange rate data from the CNB.
- `Program.cs`: Entry point of the application.

## Requirements
- .NET 9.0 or higher
- Internet connection to access the CNB data source
