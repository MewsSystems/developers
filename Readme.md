# Exchange Rate Updater Console App

> A .NET console application that fetches and filters real-time currency exchange rates from the Czech National Bank (CNB), with built-in caching and configurable HTTP client support.

---

## Table of Contents

- [Exchange Rate Updater Console App](#exchange-rate-updater-console-app)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Features](#features)
  - [Usage](#usage)
  - [Configuration](#configuration)

---

## Overview

This console application retrieves foreign exchange rates from the Czech National Bank (CNB) based on the date and selected currencies. It uses an extensible and testable design with dependency injection, caching, and custom HTTP handling. Cache is configured to expire at 14:30 every working day when rates are updated by CNB as stated [here](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/).

---

## Features

- Fetches exchange rates for specific currencies and dates
- Supports caching to avoid redundant API calls
- Pluggable architecture with interfaces and DI
- Logs detailed information using `ILogger`
- Configurable via `appsettings.json` or DI container

---

## Usage

```
dotnet run --project ExchangeRateUpdater.ConsoleApp
```

## Configuration
Make sure your appsettings.json (or equivalent config) includes:
```
{
    "CNBExchangeProviderUrl": "https://api.cnb.cz/cnbapi/exrates/daily",
    "TargetCurrencies": ["USD","EUR","CZK","JPY","KES","RUB","THB","TRY","XYZ"]
} 
```