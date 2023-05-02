# Exchange Rate Updater - .Net Backend Task

## Tools Used
- .Net 6 (Web API)
- HttpClient
- NUnit

## Development Process
To get started I first created a new .Net Web API project. I have gone for version .Net 6 as the code for the initial task was also in that version.
- I have created a controller which is calling the GetExchangeRates in the ExchangeRateProvider class.
- I implemented the GetExchangeRates method to go to the source get the rates using HttpClient.
- I then created another class called ExchangeRateFormatter in which a method is being called (FormatExchangeRates) which gets the data from the GetExchangeRates method and formats them by going through each line.
- I then added some error handling in both classes to handle incorrect data.
- Lastly I added some unit tests.
- Once the project was working fine, I removed the original files that came in the project and left only the new API project I created and the Tests project.

## Future Improvements/Changes
- More unit tests
- Handle wrong currency typed
- Source URL is currently in the appsettings.json in order to get pulled if someone pulls my code, otherwise it should in the appsettings.development.json or any other environments.