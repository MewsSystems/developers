import CurrencyPair from "./Pair";
import Currency from "./Currency";
import Trend from "./Trend";

export type CurrencyPairWithId = Record<string, CurrencyPair>;

export interface StoreShape {
  currencyPairs: CurrencyPairWithId;
  configLoaded: Boolean;
  firstRatesLoaded: Boolean;
  /* constructor(
    currencyPair: CurrencyPairWithId,
    configLoaded: Boolean,
    firstRatesLoaded: Boolean
  ) {
    this.configLoaded = configLoaded;
    this.currencyPair = currencyPair;
    this.firstRatesLoaded = firstRatesLoaded;
  } */
}
/* var curr_a = new Currency("Dollar", "USD");
var curr_b = new Currency("Euro", "EUR");

var pair = new CurrencyPair([curr_a, curr_b], true, 1.2, Trend.STABLE);

var store = new StoreShape({ aasdads: pair }, true, true);
store.currencyPair.aasdads.currencies[0].code */
