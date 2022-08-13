# ExchangeRateUpdater Test

To the best of my knowledge, the URI to retrieve required data is: `https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml`

I've rearranged the solution, moving project's files to a separate folder, so that we can clearly separate the test from its tests.

## Implementation notes

I tried to keep the code as close to the original as possible, and all changes have been limited to `ExchangeRateProvider` and its instantiation in `Program.cs`. 

To make the code more maintainable (IMHO), a few supporting types have been added; for some of these components, tests have been implemented (in new project `ExchangeRateUpdater.Tests`); this should help minimize changes should updates to the remote service occur.

A few tests for `ExchangeRateProvider.GetExchangeRates` have been added, too.  
