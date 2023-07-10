## Assumptions
- The goal is to have a code that is ready for a production environment.
- The Czech National Bank exchange rates API used for development is the one that is published at [CNB REST API URL](https://api.cnb.cz/cnbapi/swagger-ui.html).
- The REST APIs used for development are: **/cnbapi/exrates/daily** and **/cnbapi/fxrates/daily-month**.

## ADR (Architectural Decision Record)
- Do not upgrade the current .NET Core version due to possible incompatibilities with the versions installed in the production machines.
- The code will be developed following a clean architecture (Onion Architecture).
- All the code will be developed following an asynchronous pattern (async/await).

## Steps Followed
- The code has been developed following iterative rounds using TDD.
- SOLID principles have been followed.
- The folder structure has been modified to have the .csproj files at a different level than the .sln file, allowing for an organized folder structure for new projects added to the same solution file.
- As an example, a containerized REST API project has been added with just one method that provides the exchange rate for one currency:
  - Dependency injection is configured in this project.
  - You can check it by setting this project as the "Startup project".
  - You can run this project using containers by following these steps:
    ```sh    
    docker build -t exchangerate .
    docker run -p 3001:80 exchangerate
    ```
  - Access the containerized app by using localhost:3001/swagger/index.html.


## Improvements
- To avoid noise in the code in the current solution, there are no logs, but it should be a best practice for production.
- Another necessary improvement is to add a .NET Core application exception middleware.
- Add the CBn base URLs as parameters in an appsettings file.
- Considering that the daily rates change [every working day at 14:30](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/) and the monthly rates change on the [last working day of the month](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/), implementing a "system cache" could be a solution to avoid calling these REST APIs several times to retrieve the same information. The design of the system cache will depend on the use case requirements; it could be an in-memory cache, Redis, or just a table within our application that loads this data based on a scheduled time. Depending on the approach taken, we should consider using Polly or another resilience library for retrieving information from external sources.