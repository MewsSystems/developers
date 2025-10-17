# Overview

The Mews take home challenge was a unique challenge based on querying an https endpoint via an console application. A note to mention here is the API responses return data in txt format as opposed to JSON which is generally used across HTTP APIs. Hence the file was meant to be read and parsed on that basis.

The solution implements the following:

- `HttpClientAdapter.cs` : queries the URLs for `DailyRate` and `fx_rates` and reading the result as a Stream.
- `ExchangeRateProvider.cs` : Calls the `httpClient` safely using Disposable methods to avoid memory leaks. Reads the file line by line and formats the result using string manipulation and finally stores the valid results from the table into a `List` object.

# Set up instructions

```bash
git clone https://github.com/sraxler/developers.git
cd jobs/Backend/Task
dotnet restore
dotnet build
dotnet run
```

# Principles used

- KISS: Keep it super simple - the task required one function that could be broken into smaller objectives but its key to keep your code readable. As a result, I implemented a simple solution that mostly relies on 2 files.
- Inversion of Control  - To ensure object lifetimes through the length of the program and to safely initiate them from the Main function saves overhead of having to define a new object a constructor.

# Architecture overview

![ExchangeRateProvider system design](attachment:c5bd5aa2-a5fd-4858-a6aa-7684ecbeee3e:image.png)

ExchangeRateProvider system design

# Result

![image.png](attachment:7bb2c428-4337-417b-9262-dbcfc4089109:image.png)

# Future work

- Swap `HttpClient` for `HttpClientFactory` if console app requires making multiple API calls for a recurring application that stays live until it is stopped.
- Create a text file to store data on Local machine using the response to avoid querying the API multiple times as the website states daily rates are updated on business days at 2:30 pm and fx_rates are updated once a month. Since the data is not too large and caching would work only if the application has a longer runtime - text files would suffice.