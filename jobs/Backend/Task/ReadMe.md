# Mutation Testing with Stryker.NET
This project uses **Stryker.NET** for mutation testing to ensure the quality of the tests.

## Prerequisites
- Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed.
- Install **Stryker.NET** globally using the following command:
  \\\bash
  dotnet tool install -g dotnet-stryker
  \\\

## Running Stryker.NET
1. Open a terminal and navigate to the `ExchangeRateUpdater.Tests` directory:
2. Run Stryker.NET with the following command to start mutation testing and automatically open the result report in your browser:
   \\\bash
   dotnet stryker -o
   \\\

   This will:
   - Analyze the test project.
   - Generate mutants for the code.
   - Run the tests to determine how many mutants are killed (detected by the tests).
   - Generate an HTML report and open it in your default browser.

## Viewing the Results
After running Stryker.NET, the mutation testing results will be displayed in your browser.
For more information about Stryker.NET, visit the [official documentation](https://stryker-mutator.io/).
