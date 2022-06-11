# Mews Backend Developer Task - Exchange Rates

## Specification

The task is to implement an ExchangeRateProvider for Czech National Bank. Find data source on their web - part of the task is to find the source of the exchange rate data and a way how to extract it from there.

It is up to you to decide which technology (from .NET family) or package to use. Any code design changes/decisions to the provided skeleton are also completely up to you.

The solution has to be buildable, runnable and the test program should output the obtained exchange rates.

Goal is to implement a fully functional provider based on real world public data source of the assigned bank.

To submit your solution, just open a new pull request to this repository.

Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.

## Tasks

- [x] Find data source on their web - part of the task is to find the source of the exchange rate data
- [x] Choose technology for implementation
- [ ] Add unit tests for the exchange rate provider
- [ ] Implement unit tests with mock data provider
- [ ] Add unit tests for the real world data provider retriever/parser
- [ ] Implement real world data provider and example CLI
- [ ] Perform acceptance tests per below listed acceptance criteria
- [ ] Add build, test and usage instructions to readme

## Acceptance Criteria

- [ ] Solution is buildable and runnable
- [ ] Test program outputs the obtained exchange rates
- [ ] Provider is fully functional and sources real world public data from CNB
- [ ] Code is of a long-term maintainable production standard

## Data Source

<img alt="CNB logo" src="https://www.cnb.cz/export/system/modules/cz.nelasoft.opencms.cnb/resources/img/LOGO-2RA_RGB.svg" width="200px" />

[overview](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/)

[daily text file](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt)
