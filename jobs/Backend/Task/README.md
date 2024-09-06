# Exchange Rates

Console Application to print exchange rates for various currencies using data from Czech National Bank for the current day.

## Getting Started

1. In project root, run `docker build -f ExchangeRateUpdater.ConsoleApp\Dockerfile -t exchangerateupdater .` to build docker image.
2. Run `docker run exchangerateupdater` to start the container.

## Future Improvements (when application expands)

1. Caching for HttpClient
2. Logging framework to write logs externally
3. Transient fault handling
4. Localisations for messages
5. CQRS design pattern