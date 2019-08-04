// @flow strict

export type Currency = {|
  +code: string,
  +name: string,
|};

export type CurrencyPairApi = {
  [key: string]: [Currency, Currency],
  ...,
};

export type CurrencyPair = {|
  +id: string,
  +currencies: [Currency, Currency],
  +rates?: number[],
|};

export type State = {|
  +currencyPairs: CurrencyPair[],
  +filteredCurrencies: [],
  +isLoadingConfig: boolean,
  +fetchConfigError: ?Error,
|};

export type Action = {|
  +type: string,
  +payload: {|
    +currencyPairsApi?: CurrencyPairApi,
    +error?: Error,
  |},
|};

export type Dispatch = (action: Action) => void;
