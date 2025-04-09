# Mews developers
Welcome to my attempt at creating the Exchange Rate Updater! It was a challenge with my lack of C# experience, but I'm pleased with the end result!
> Note: The actual code is somewhat buried by all the folders, it can be found at: `jobs -> Backend -> Task`

## Input file🗒️
The program expects the first argument to be the path to a text file listing the currencies you want exchange rates for (in 3-letter ISO format). The currencies can be separated by spaces, commas, pipes, newlines or tabs.

> If no arguments are provided, it defaults to "currencies.txt". 

> In this solution, the first argument is set to "argumentsTest.txt". To change this, go to:
> 
> `Project -> Properties -> Debug -> General -> Open debug lauch profiles UI -> Command line arguments`.

## How the program works⚙️
1. Reads and parses the currencies from the input file.
2. Fetches the current exchange rates from the Czech National Bank, using [their API](https://api.cnb.cz/cnbapi/exrates/daily?date=2025-04-10).
3. Parses the JSON response into an array of ExchangeRate objects.
4. Prints the exchange rates to the terminal.
5. Logs a warning for any currency whose rate could not be found.

## Screenshots📷
### A simple input file:
![image](https://github.com/user-attachments/assets/5236e1bd-5a35-442b-abdd-9939c84894d0)
### Its output:
![image](https://github.com/user-attachments/assets/5ba9ed58-db67-4d16-bf9e-de4789a261c5)
