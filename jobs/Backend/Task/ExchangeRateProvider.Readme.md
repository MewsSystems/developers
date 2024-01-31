# ExchangeRateProvider

Implementation of BE developer task - `ExchangeRateProvider` -  component for reading Czech National Bank's (CNB) exchages rates.


## Structure

The solution consists of 4 projects projects:

### ExchangeRateProvider

The *bussiness* part of the implementation, without any implementation details of the CNB API.

### ExchangeRateProvider.CnApi
Contains classes dependent on the CNB Api implementation.

### ExchangeRateProvider.Console

The original `Program.cs` to allowing test with real data. 

### ExchangeRateProvider.Tests
Unit tests for expected *business* behaviour.


## Open issues.
CNB release new exchange rates each bussiness day after 14:30 of local time. (https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/)

To optimize the caching, there is needed:
-  proper detection of bussines days (excluding current year public holidays).
- The configured release time should be better saved in local time and based current timezone at CZ, converted to UTC. 





