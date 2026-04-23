# Exchange Rate Updater

This project retrieves and displays exchange rates for a predefined set of currencies from specified sources.

## Prerequisites

To run this project, you need to have the following installed on your system:

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)

## Installation

1. **Install .NET 7**:
   - Visit the [.NET 7 download page](https://dotnet.microsoft.com/download/dotnet/7.0).
   - Download and install the appropriate version for your operating system.

2. **Verify Installation**:
   - Open a terminal and run:
     ```bash
     dotnet --version
     ```
   - Ensure the output shows a version starting with `7`.

## Running the Program

1. **Clone the Repository**:
   - Clone this project to your local machine:
     ```bash
     git clone <repository-url>
     cd <repository-folder>
     ```

2. **Restore Dependencies**:
   - Run the following command to restore any required dependencies:
     ```bash
     dotnet restore
     ```

3. **Run the Program**:
   - Execute the program using:
     ```bash
     dotnet run
     ```

4. **Output**:
   - The program will fetch exchange rates and display them in the terminal.

## Configuration

The program uses an `appsettings.json` file to specify the URLs for exchange rate sources. Ensure the file is present in the project directory and contains the following structure:

```json
{
  "ExchangeRateSources": {
    "CommonCurrencies": "https://www.cnb.cz/en/financial-markets/foreign-exchange-marketcentral-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
    "OtherCurrencies": "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt"
  }
}
