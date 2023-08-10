# Mews backend developer task (.NET) notes

Im not fan of unnecessary complexity and try to follow YAGNI principle. Overengineering and trying to have everything generic could end up with unreadable code.   Some consideration could be found under the todo in the code.

### Considerations
- For HTTP there could be retry mechanism
- Could be achieved with DI and AddTransientHttpErrorPolicy for example
- For HTTP there could be caching mechanism
  - My experiences with caches are, that they could safe a resources, but they are also source of bugs, especially when synchronization is needed.
  - I dont know how big load is expected. If it is small, then there is no need for cache.
  - Here we can use the cache for currency fetches and also for validations, where we could parse all available currencies for national bank.
- Dependency injection could take place here, but there is a few dependencies, which are easy to propagate and is not necessary to set lifetime.
  - With DI Logging and Configuration would take place.
  - It would be possible to expose ExchangeRateService builder for another applications, which would like to use this service.
- Some examples data (KES) are not in the Czech National Bank data source. If we want to support every currency, then we need to find another data source.
- I didnt divide files into folders in solution, because there is only a few. 