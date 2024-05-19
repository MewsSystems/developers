# Mews backend developer task - David Rimar

This is a .NET API project allowing clients to query a list of exchange rates with the Czech Korona as the target currency.

### Notes:
- Completely aware that most of the unit tests are missing
- Because the CNB API is public, no authentication takes place between the ExchangeRateUpdater API and the CNB API
- Debatable why I went for an API style project, thought it would be more realistic
- Because it is an API endpoint to get exchange rates, there is Fluent Validation on the supplied currencies

### Discussion Points:
*Point1* Extendibility: Allow the client to specify a base currency beside the list of currencies, enabling the extension to call
other bank's APIs to fetch exchange rates based on the requested base currency. How could this be done?
	- CnbExchangeRateProvider class?
	- How do we do a switch based on the base currency?

*Point2* API Design: Because this turned out to be an API, the GET exchange rates endpoint expects a list of currencues as query parameters. It raises a question how long an URI can be..

*Point3* API Authentication: Implement authentication between APIs in production environment

*Point4* Storing Configs: Currenctly in appsettings.json but normally it would come from Azure Table Storage for example. 