# Solution Description

I have decided to implement this task using [ASP.NET](http://ASP.NET) since it enables to use of the implemented application across different platforms by using the exposed API or in case it will be required to use it in some other C# project it is possible to use `NSwag`  to automatically generate client.

The solution is implemented using the `Onion Architecture` which doesn't make much sense for such a small project and brings more unnecessary complexity. This architecture shines in bigger projects due to its separation of concerns which results in better maintainability.

The final solution still needs a bit more love in order to make it perfect but it works reasonably well now. Here is a list of things that are included in the current solution and things that are missing:

Implemented features:

- optimized cache since the data from the CNB API are refreshed every working day at 14:30 I implemented a caching mechanism that gets the data once a day, handles weekends, and gets the new data as soon as possible.  This also required handling different time zones.
- optimized API calls - I've decided to save data for all currencies at once to my cache using one call and then serve these data to users. This results in 1 API call a day or even less during the weekends
- there are two endpoints implemented
    - **`http://localhost:5204/api/v1/exchange-rates/USD`**
        - either returns successfully the requested exchange rate or `HTTP error code 400` when a user enters an invalid currency code or `HTTP error code 404` when the currency code is valid but not found
    - **`http://localhost:5204/api/v1/exchange-rates`**
        - The user can enter comma comma-separated list of currency codes e.g. `USD, EUR, JPY` and it returns a list of requested exchange rates.
        - if one of the codes is in incorrect format, the endpoint returns `HTTP error code 400`
        - if some of the codes are not found in the CNB database, it just skips it

Missing features:

- I skipped implementing the handling of holidays in my caching mechanism
- service health-check
- proper exception handling
- more robust calling mechanism for fetching data from CNB API
- more tests :)
