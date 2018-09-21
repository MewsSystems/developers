// @flow strict

export type Data = {|
  [id: string]: {|
    currencies: Array<{| +code: string, +name: string |}>,
  |},
|};

export type SelectedRates = string[];

export type Rates = {| [id: string]: {| +current: number, +before: number |} |};

export type StateTypes = {
  +isFetchingConfig: boolean,
  +isFetchingRates: boolean,
  +rates: Rates,
  +selectedRates: SelectedRates,
  +data: Data,
  +APIerror: ?Error,
};
