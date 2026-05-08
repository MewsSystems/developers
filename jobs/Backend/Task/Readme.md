# Exchange Rate Updater (CNB Provider)

This is a .NET 8 console application that fetches daily exchange rates from the **Czech National Bank (CNB)** and prints the configured currency rates against CZK.

---

## 🚀 Features

- Fetches **real CNB daily rates** from official `.txt` feed.
- Automatically detects **decimal separator style** (`,` or `.`) from the file.
- Parses and computes **per-unit exchange rates** (`rate / amount`).
- Supports configurable currencies, timeout, and URL.
- Caches parsed data for faster repeated runs.
- Implements DI, logging, and options validation for production use.

---

## ⚙️ Configuration

All settings are defined in `appsettings.json`:

```json
{
  "ExchangeRateSettings": {
    "CnbDailyUrl": "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
    "HttpTimeoutSeconds": 10,
    "Currencies": [ "EUR", "USD", "GBP", "INR" ]
  }
}
