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
  +filteredCurrencyPairs: string[],
  +isLoadingConfig: boolean,
  +fetchConfigError: ?Error,
|};

export type Action = {|
  +type: string,
  +payload: {|
    +currencyPairsApi?: CurrencyPairApi,
    +error?: Error,
    +filteredCurrencyPairId?: string,
  |},
|};

export type Dispatch = (action: Action) => void;
