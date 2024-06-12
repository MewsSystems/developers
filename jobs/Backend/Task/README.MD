# Czech National Bank Exchange Rate Provider

## Architecture

There are 3 projects in the solution - the original ConsoleApp, the Application library which contains the main functionality, and a Test project for the Application library.

This is structured in this way to allow for future extensibility, for example it would be easy to replace the Console App with an API project.

## Libraries

Libraries used include the following:

- [Autofac](https://autofac.org/) - allowing assembly scanning for dependency injection
- [FluentValidation](https://fluentvalidation.net/) - for validation of input

## Future Improvements

    - May want to think about caching the result from the CNB Api, as it is only updated once a day.  
    - Another possibility is fetching it automatically every day and storing in a database.  Could use an Azure Function, or background task if there is an app service.
    - Presumably the data won't be consumed by a Console App in the real world, should there be an API?
    - Implement a Mediatr / CQRS pattern
    - CI/CD pipeline
    - Send logging to a logging server or Application Insights




