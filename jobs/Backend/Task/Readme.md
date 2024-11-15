# Backend Task
Inside these changes you'll be able to find 2 csprojs where we can run all our ExchangeRateUpdater application, one of them is a single Console app, the other one is an REST Api.

What I've applied:
- Clean Architecture
- Mediator pattern.
- CQS pattern.
- Options pattern in order to access the configuration data.
- Retry policy using Polly for ExchangeRateApiClient.

Missing code:
- Unit tests and integration tests (due to personal matters I could not add all the required test coverage)
- Missing Unit tests: GetExchangesRatesByDateQueryValidatorTest, GetExchangesRatesByDateQueryHandlerTest.
- Missing integration tests: GetExchangeRatesByDate endpoint test.

Improvements:
- Add Background service or CronJob that will update memory cache every X time, with this we'll apply 100% the CQS pattern because right now GetExchangesRatesByDateQueryHandler is muttating the system adding the data on memory cache in case doens't exist.
- In case when every time the exchangerate changes and other systems requires to know how it changed I'd apply an event driven system in order to send events to some RabbitMQ queue in order to notify an internal application that will send events to external systems per example using Kafka or Azure eventhub.
- Instead of using memory cache use a distributed cache like Redis.
