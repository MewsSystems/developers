# Task

This PR addresses the requirement to implement an exchange rate provider for the Czech National Bank, using their publically available APIs to source
the exchange rate data for specifically requested currencies. The currencies are to be returned to the user in a console application.

## Approach

I have approached this task with the idea of creating an MVP that satisfies the requirements. That means that I have retained the console application, and extended
it's functionality to return the required data. My main focus was to keep the code clean, clear and SOLID so as to be easily maintainable and extensible. I had
considered replacing this console application with a minimal api for example, but in the end decided that my focus here should be on delivering the requirementsas as 
written. I expect that this program would eventually service a number of front ends and at that point, adding an api would be more appropriate.

I have implemented logging and caching - these are simple implementations appropriate for a console application. For logging, I have kept log messages clear and simple,
with only one log instance requiring structued logging. These logs are simple console logs. In a production app, I would expect to connect to a provider such as
application insights. Similarly, for the caching that is a simple in memory cache - it would be preferable to implement a seperate caching source running independantly
from the main application, such as Redis for example, to increase the resiliance of the cache. I have set a short time to live for the purposes of this test, but in a
real world solution would probably look to refresh the cache inline with source API.

I have added unit tests for updated Provider and Service classes. For the sake of brevity I have used multiple asserts in each test scenario, as I believe that keeps test
code clearer and easier to read, without sacrificing testing the classes logic.

More generally, I have updated the project to .net 8, and brought the program.cs file in line with modern coding approaches. I have used global usings to reduce 'noise'
in files, and have used file scoped namespacing to make the code easier to read. I have used IHttpClientFactory to create the HttpClients and typed them, as well as
added standard resiliance handling to deal with failed calls.

## Improvements

- Implement external services for logging and caching
- Accept user input to search for specific currency or a list of currencies, for specific dates
- Provide an API
