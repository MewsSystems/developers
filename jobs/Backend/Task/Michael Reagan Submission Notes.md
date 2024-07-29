# Michael Reagan's submission for the .NET Backend Developer task

> In order to run the solution you will need to run the "Update-Database" command against the ExchangeRateUpdater.Infrastructure project and have a local instance of MSSQL installed.  Also will need to set WebAPI project as Startup Project

This solution is my submission to the implementation of the ExchangeRateProvider built upon a Clean Architecture pattern.

I've opted for a Web API application so that the data can be easily accessed by different front-end or mobile applications as well as other back-end services.

In order to make the solution as "production ready" as possible I've implemented features like Serilog for extended logging, Swagger Documentation for easier development, set up API versioning for future updates, adding global exception handling, CORS policy for future front-end applications, and various other features.

I've added appropriate Test cases for the key services involved in the solution.

I opted to set up a database for this project using Entity Framework and MSSQL Server to make the application more dynamic and data driven. This allowed me to create a CurrencySource table to house the API information for the Czech National Bank so that the exchange rate data process can be easily modified or extended. In addition, having the database set up allowed me to use the .NET identity provider to add more security to the application including user registration, login, logout and access via Access and Refresh tokens. All part of preparing an application to be production ready for long-term use.

I look forward to discussing my solution in more detail.

Submitted July 28, 2024
