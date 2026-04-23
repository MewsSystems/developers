# CNB API - Daily Exchange Rates

Base URL

- `https://api.cnb.cz/cnbapi/exrates/daily`

Description

- This endpoint returns the full daily foreign exchange rates published by the Czech National Bank (CNB).
- The endpoint does not support filtering by currency: every request returns the whole set of available currency entries.
- Each entry contains a `validFor` property that indicates the date the rate is valid for.

Update schedule & behaviour

- Rates are published/updated on working days (weekdays) around 14:30 UTC (I will asume its Zulu time since they do not specify) according to the CNB documentation: https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates.
- If the API is queried on a weekend or holiday, the response contains the most recently published rates (the latest `validFor` date prior to the request).

Response shape (representative)

```json
{
  "rates": [
    {
      "amount": 0,
      "country": "string",
      "currency": "string",
      "currencyCode": "string",
      "order": 0,
      "rate": 0,
      "validFor": "2026-02-15"
    }
  ]
}
```

Field meanings

- `amount` (decimal): how many units of the foreign currency the `rate` corresponds to.
- `rate` (decimal): amount of CZK for the given `amount` units of the foreign currency.
  - To obtain the per-1-unit rate in CZK: `rate / amount`.
- `currencyCode` (string): three-letter ISO 4217 currency code (e.g. `USD`, `EUR`).
- `currency` (string): localized currency name.
- `country` (string): country name associated with the currency.
- `order` (int): ordering index in the returned list (not used for calculations).
- `validFor` (date string): the calendar date that this rate is valid for (format `yyyy-MM-dd`).

Errors

- Error responses follow a small shape similar to:

```json
{
  "description": "...",
  "endPoint": "...",
  "errorCode": "INTERNAL_SERVER_ERROR",
  "happenedAt": "2026-02-15T15:46:07.643Z",
  "messageId": "..."
}
```

References

- CNB rates format FAQ: https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates
- CNB daily rates endpoint (used): https://api.cnb.cz/cnbapi/exrates/daily
