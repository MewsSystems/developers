# Mews backend developer task

The task of implementing an ExchangeRateProvider for Czech National Bank seems relatively simple on the surface but has some gotchas and is open-ended enough which gives me some good oportunities to think of and apply various dotnet best practices and design patterns. At the same time this also bears the risk of over-engineering.


### Functional Requirements
The main functional requirement is well defined with a clear example in Program.cs

- The 'solution' should return current exchange rates for given currency codes that CNB provides data and ONLY those. Exclude unsupported or missing currencies.
- The source of the exchange rate must be CNB
- The TargetCurrency we will have exchange rates for is CZK
- Our Result model should have SourceCurrency, TargetCurrency, Value as minimum
- The cnb.cz source has the exchange rates from SourceCurrency (USD,POUND etc) to target currency (CZK)
and we do not need to calculate and return the opposite (CZK to USD)
- The cnb.cz source SourceCurrency sometimes has amount 1 (USD,POUND etc) and sometimes has amount 100 (TRY,THB). For our return model we will simplify and base every exchange rate to 1 SourceCurrency.

- Result Model
```
- `SourceCurrency`
- `TargetCurrency` (CZK)
- `Value` (rate value normalized per 1 unit)
- `ValidFor` (the CNB bulletin’s  date)
```


### Non-Functional Requirements

For non-functional requirements  I will have to take some liberties and make some assumptions on the basis of the solution being complete enough for a "production environment".

- Avoid fetching from CNB on every request. I will need to come up with some caching strategy

- Resilience. Retry transient errors (HTTP 5xx, 429) with backoff (e.g., via Polly). Optional: fall back to TXT/XML source if the API is unavailable.

- Maintainability & Extensibility. I will try to make good use of patterns like strategy+factory combo which seems like a good match for the task and setup the project for future extensibility. I need to split application logic from any 'client'(Api, Console app) and setup the project structure accordingly. I will try to aim for 100% Unit Testing. I need to have good logging coverage accross the application.

- Api. I will try to expose the ExchangeRateProvider via an API and deploy it all the way to "production".

- For a real-world deployment, the solution should be containerized and deployable via CI/CD pipeline (GitHub Actions or Azure DevOps), though this may be beyond the scope of the exercise.

## Gotchas & Assumptions

- I need to identify the best source possible from Czech National Bank. The task emphasizes on finding data source and extracting the data. After investigating it seems that CNB offers multiple ways to access the data 

- Those are 
    - API (https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates/dailyUsingGET_1) 
    - Text file (https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt) 
    - XML (https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml)


By far the simplest and more standard/best practice way seems to be using their API. The text file is also relatevly very simple and can be parsed easily with a package like csvhelper which I have used extensively. A possible implementation could have both ways to create some resiliency and fall back in case of API (and all retries) failing. Thats is something to consider but for now I will use the banks API.

- The solution should be able to balance handling load but also making sure to not serve stale data.

This could be potentially critical. On one hand I wouldnt like the idea of requesting the API or downloading file for new data for every request. On the other hand I wouldnt like to serve stale data. I will need some caching strategy. At the bank website it states

"Exchange rates of commonly traded currencies are declared every working day after 2.30 p.m. and are valid for the current working day and, where relevant, the following Saturday, Sunday or public holiday (for example, an exchange rate declared on Tuesday 23 December is valid for Tuesday 23 December, the public holidays 24–26 December, and Saturday 27 December and Sunday 28 December)."

But... what does "after 2:30 pm" means,when excactly, after 2:30 can be 3,4,6pm?

I will make the assumption that the data are available sometime between 2:31pm-3:31pm Prague Time and refresh cache every 5 minutes starting at 2:31pm until 3:31 pm. After that I will assume the data are stable and refresh cache every hour. I will reduce to 12 hours on weekends and ignore holidays for siplicity. In case of failing to get fresh data I will allow some cached data to be served.


- Unsupported currencies: If a requested currency is not present in the CNB publication or is not a currency code it is silently ignored (not returned as an error).
