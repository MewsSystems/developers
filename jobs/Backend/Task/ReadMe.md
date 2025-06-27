## Improvement List

1. **Add Dependency Injection** - (If needed) e.g. for using in web projects.
2. **Create Unit Tests** - Add comprehensive test coverage with mocking to ensure code reliability and prevent regressions
3. **Implement Custom Exceptions** - Replace generic Exception throws with specific domain exceptions for better error handling
4. **Add Retry Policies** - Implement automatic retry logic for API calls to handle temporary network failures
5. **Implement Input Validation** - Add proper validation for currency codes and exchange rate values before processing
6. **Add Caching** - Implement response caching to reduce API calls and improve performance since exchange rates don't change frequently
7. **Add more generic methods/classes** - Add more flexibility to reuse classes for other available open APIs
8. **Use HTTP Client Generation** - Consider using libraries like Refit or NSwag to auto-generate strongly-typed HTTP clients from API specifications

