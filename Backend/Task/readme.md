# Notes
## Architecture
* I think it will be better to use Enum of currencies instead of class for the entity object
* I've used Castle Windsor as DI, but in .Net Core I'm using Microsoft DI and I  also have partial experiences with Autofac
* Project has target version of .NET Framework 4.5.1 I've set it to 4.7.1, because there you can use many advanced features of C# 7.* including Value tuples and so, but didn't need it at all 

## Possible improvements
* Caching on ExchangeRateProvider (depending on expected load). Exchange rate data from ČNB are valid for 24 hours (https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/) and it's not necessary to download it for each request.
* Logging
* Extend ExchangeRate model for different quantities in rates e.g. rate for Hungarian forint is specified per 100 forints (currently implemented as the calculation), but it depends on the main purpose of ExchangeRateProvider (just display exchange rate or using it for some calculations)
* There is a possible performance issue in the original code. Variable rates in Program.cs ln:35 is enumerated two times (ln:37, ln:38). It's better to use ToList() function before you start to use the variable.