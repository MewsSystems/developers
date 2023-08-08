# Max Aldunate - August 2023 - Mews backend developer task (.NET)

#### Startegy
Overkill solutions is often a big problem in software development.
I usually follow the rule of:
1. At first implementation make it as simple as possible
2. Second time think about whether abstraction is necessary
3. Third time an abstraction is mandatory!

Although this is an exercise, I have tried to follow this strategy and make the decisions made clear in this document.

#### Cache
Taking into account that the small volume of data, that I don't know the number of calls to make, I have not implemented a cache, I have only declared the object. But since the information is updated once a day and only on working days, it should be implemented by scheduling the update accordingly, for example with Quartz.

#### Test Project
I have added a test project `ExchangeRateUpdaterTests` with the intention of including tests, although the logic is not very complex, I have implemented different cases.

#### Dependency Injection
I have decided not to implement the dependency injection to avoid making more changes on the base project but I consider it essential production. In any case, the interfaces are declared and the classes are injected manually.

#### Logger
In the same way as the previous point, I have not implemented it, although I consider it essential to production.

#### Configuration file
e.g. for CNB API URL

#### IHttpClientFactory
`IHttpClientFactory` should be used instead of `HttpClient` when dependency injection is implemented

#### Handle exceptions
For example when external CBN API is called

#### API Call return a List instead of IEnumerableAsync
In `CnbApiWrapper.GetExchangeRatesAsync` I have used a `List` as the return value instead of `IEnumerableAsync` because we need all the data in memory before using it.

#### One Class one file rule
I have followed this rule with the exception of the declaration of interfaces and related model structures, in both cases it seems more readable to me

#### Main to Async
I have converted the `Program.Main` method to `async Task` because of the nature of the HTTP calls