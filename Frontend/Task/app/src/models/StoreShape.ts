import CurrencyPair from "./Pair";

export type CurrencyPairWithId = Record<string, CurrencyPair>;

export interface StoreShape {
  currencyPairs: CurrencyPairWithId;
  configLoaded: Boolean;
  firstRatesLoaded: Boolean;
}
