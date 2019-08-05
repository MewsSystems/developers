// @flow strict

export type Currency = {|
  +code: string,
  +name: string,
|};

export type CurrencyPairApi = {
  [key: string]: [Currency, Currency],
  ...,
};

export type RatesApi = {
  [key: string]: number,
  ...,
};

export type CurrencyPair = {|
  +id: string,
  +currencies: [Currency, Currency],
  +rates: number[],
|};

export type State = {|
  +currencyPairs: CurrencyPair[],
  +filteredCurrencyPairs: string[],
  +isLoadingConfig: boolean,
  +fetchConfigError: ?Error,
  +isLoadingRates: boolean,
  +fetchRatesError: ?Error,
|};

export type Action = {|
  +type: string,
  +payload: {|
    +currencyPairsApi?: CurrencyPairApi,
    +error?: Error,
    +filteredCurrencyPairId?: string,
    +ratesApi?: RatesApi,
  |},
|};

export type Dispatch = (action: Action) => void;
