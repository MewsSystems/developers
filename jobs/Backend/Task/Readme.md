# Exchange rate updater solution

Original requirements for this solution are described here: [DotNet task](../DotNet.md)

## Business analysis
We could say CNB (Czech national bank) exports data in 2 different ways now, based on discussion here: https://developers.cnb.cz/discuss/633c52c8da0e79003c33a2c1
* the "new" REST json api - https://api.cnb.cz/cnbapi/swagger-ui.html
* the "old" txt/html format e.g. - https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt
  * formats are described here: https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/
  * all links to these could be find through the website - https://www.cnb.cz/en/financial-markets/foreign-exchange-market/

I assume these additional business requirements (usually discussed with team, PO, etc):
  - the goal is fetching newest exchange rates from czech national bank with possible filtering via currency codes
  - commonly traded currencies is enough for now (other currencies added probably in the future)
  - only few requests per day should be supported (REST api without Redis or containerization should be fine)
  - company has Azure resources (for central monitoring, logging, configuration - e.g. KeyVault, deployment to Azure Web App)
  - no need for permanent storage
  - no need for authorization/authentication (private network)
  - output data will be displayed in some other UI (simple format like "USD/CZK = 23.531" is good enough now)
  - most of the apps in company are written in MVC, developers are used to think more in onion architecture (instead of clean architecture, hexagonal architecture)
  - MediatR, Command & Query with handlers is probably a little "overkill" and most developers in our company are still not used to it. But it is possible to refactor this to these different approaches.

## Architecture decisions

Here are decisions and thoughts about the implementation.

### Which type of application?

The current app is the newest (.Net 8 is now) **web api with minimal api** service. The default console app is usually good for quick prototypes but it is difficult for input/output of data. Usually services are the most used types of apps in production. Since REST is the most common nowadays I chose this approach. 
There are tons of other alternatives like (Web app Blazor/Razor/gRPC service, MVC, etc.) and they could be a better choice depending on ExchangeRateUpdater integration to the current infrastructure (unknown for me at this moment) or more specification from business. 

### Solution structure & architecture

As described in business expectations - **Onion architecture** approach is used. Only the most known/common technologies/packages are used. It could be refactored to MediatR approach in the future (depends on the learning curve for other colleagues).
  => Client request received -> Rest Api method found -> request data mapped to DTO/domain objects -> api business service called with DTO/domain object -> other dependency services called (cache, external, etc.) -> computed result mapped from DTO/domain to response data -> returned Json data back to client     
The service is still pretty simple, so only directories are used for "separation". Usually more projects are created for each of the part (subset of directories), but I considered it overkill yet.   

### Used technologies & conventions

1. Common ASP.NET Core Web Api with minimal api, swagger - from standard template
1. Auto Api registration from assembly through IApiRoute.cs interface
1. Auto service registration from assembly through IServices.cs interface
1. Logging + Monitoring -> should be in Azure AI (application insights) // Not implemented completely
1. Configuration is done through appsettings/Environment variables
1. FluentValidation with auto registration
1. AutoMapper with auto registration through Profile files
1. XUnit for unit/integration tests
1. Moq for mocking services

## Getting Started

The following prerequisites are required to build and run the solution:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)