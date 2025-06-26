## Improvement List

1. **Add Dependency Injection** - Replace manual object creation with proper DI container and HttpClientFactory to avoid socket exhaustion
2. **Create Unit Tests** - Add comprehensive test coverage with mocking to ensure code reliability and prevent regressions
3. **Implement Custom Exceptions** - Replace generic Exception throws with specific domain exceptions for better error handling
4. **Add Retry Policies** - Implement automatic retry logic for API calls to handle temporary network failures
5. **Implement Input Validation** - Add proper validation for currency codes and exchange rate values before processing
6. **Add Caching** - Implement response caching to reduce API calls and improve performance since exchange rates don't change frequently

