export interface ConfigInterface {
  currencyPairs?: {
    currency: CurrencyArray;
  };
}

export interface CurrencyArray {
  currency: Array<CurrencyInterface>;
}
export interface CurrencyInterface {
  code: string;
  name: string;
}

export interface RatesInterface {
  rates?: Array<RateInterface>;
}

export interface RateInterface {
  rate: { rate: number; trend: string };
}

export interface CurrencyListProps {
  fetchConfigInit: Function;
  rates: RatesInterface;
  config: ConfigInterface;
  filtered: Array<string>;
}

export interface CurrencyListState {
  config: ConfigInterface;
  rates: RatesInterface;
  filtered: Array<string>;
}

export interface FilterProps {
  config: ConfigInterface;
  filterCurrencies: Function;
  resetFilter: Function;
  filtered: Array<string>;
}

export interface FilterState {
  filtered: Array<string>;
}
