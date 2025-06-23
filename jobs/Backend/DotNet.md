# Mews backend developer task (.NET)

## Instructions
The task is to implement an [ExchangeRateProvider](Task/ExchangeRateProvider.cs) for Czech National Bank. Find data source on their web - part of the task is to find the source of the exchange rate data and a way how to extract it from there.

It is up to you to decide which technology (from .NET family) or package to use. Any code design changes/decisions to the provided skeleton are also completely up to you.

The solution has to be buildable, runnable and the test program should output the obtained exchange rates.

Goal is to implement a fully functional provider based on real world public data source of the assigned bank.

To submit your solution, create a Pull Request from a fork.

Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.



## Technical Decisions and Approach

This solution was approached as if it were a real-world daily task, where time and effort need to be balanced with quality and maintainability. Here are the key technical decisions and rationale:

### Pragmatic Architecture
- Used a straightforward layered architecture with clear separation of concerns through interfaces
- Focused on making the code maintainable and testable without over-engineering
- Kept the solution simple while ensuring it's extensible for future requirements

### Robustness
- Implemented proper error handling and logging throughout the application
- Added a backup mechanism to handle API downtime
- Used retry policies for HTTP requests to handle transient failures
- Included comprehensive validation of input data and configuration

### Design Decisions
- Used dependency injection for better testability and loose coupling
- Implemented the Provider pattern to abstract data sources
- Made the data source configurable (CNB API vs Backup file) without changing the core logic
- Kept the parsing logic separate to handle potential format changes

### Trade-offs
- Didn't implement a complex caching strategy as it wasn't a core requirement
- Used a simple file-based backup instead of a database solution
- Kept the configuration in appsettings.json instead of implementing a more sophisticated configuration management
- Limited the scope of unit tests to critical components

The goal was to deliver a robust, maintainable solution that solves the immediate problem while remaining flexible enough for future changes, without spending time on unnecessary complexity or premature optimization.
