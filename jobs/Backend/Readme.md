# Mews backend developer task

We are focused on multiple backend frameworks at Mews. Depending on the job position you are applying for, you can choose among the following:

* [.NET](DotNet.md)
* [Ruby on Rails](RoR.md)

---

## Design Overview

### Components

- **ExchangeRateService**
  - Orchestrates fetching exchange rates for requested currencies.
  - Checks daily rates cache first; if some currencies are missing, checks monthly rates cache.
  - Only calls the monthly API if needed, minimizing unnecessary API calls.
  - Logs warnings for currencies not found in either source.

- **CnbRatesCache**
  - Wraps `IMemoryCache` for caching daily and monthly rates.
  - Uses a unique cache key and an expiration factory for each cache instance.
  - Handles cache hits, misses, and logging.
  - Fetches and caches data using a provided factory function.

- **CzechApiClient**
  - Handles HTTP GET requests to the Czech National Bank API.
  - Logs requests and responses, and handles errors gracefully.

---

### Flow Summary

1. **User requests exchange rates** for a set of currencies.
2. **Daily rates** are fetched from cache (or API if not cached).
3. **If all requested currencies are found in daily rates:**  
   → Return results.
4. **If some are missing:**  
   → Fetch monthly rates from cache (or API if not cached) for missing currencies only.
5. **Combine results** and log warnings for any currencies not found in either source.
6. **Return the final list** to the user.

---

### Cache Strategy

- **Daily rates cache:** Expires at the next Czech bank update time.
- **Monthly rates cache:** Expires at midnight on the first day of the next month.
- **Both caches:** Use `CnbRatesCache` for consistent logic and logging.

---

### API Client

- **CzechApiClient** is injected and used by the service and cache to fetch fresh data from the CNB API endpoints.
- Handles logging and error management for all HTTP requests.

---

### Key Points

- **Efficient:** Only calls the monthly API if daily data is incomplete.
- **Testable:** All dependencies are injected, making unit testing straightforward.
- **Observable:** Extensive logging for cache hits/misses, API calls, and errors.
- **Extensible:** Easy to add more cache layers or endpoints if needed.

---

## Run The App

### In a containerised way (from Backend folder):

Build the image
```sh
docker build -t exchangerate-task -f Task/Dockerfile .
```
Run the app
```sh
docker run -it --rm exchangerate-task
```

### On Local Machine: 
```sh
dotnet restore
dotnet run --project Task
```
