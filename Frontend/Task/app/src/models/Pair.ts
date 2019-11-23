import Currency from "./Currency";
import Trend from "./Trend";

interface CurrencyPair {
  currencies: Currency[];
  shown: Boolean;
  rate: Number;
  trend: Trend;
  /* constructor(
    currencies: Currency[],
    shown: Boolean,
    rate: Number,
    trend: Trend
  ) {
    this.currencies = currencies;
    this.shown = shown;
    this.trend = trend;
    this.rate = rate;
  } */
}
export default CurrencyPair;
