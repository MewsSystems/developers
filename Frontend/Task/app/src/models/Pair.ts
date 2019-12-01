import Currency from "./Currency";
import Trend from "./Trend";

interface CurrencyPair {
  currencies: Currency[];
  shown: Boolean;
  rate: Number;
  trend: Trend;
}
export default CurrencyPair;
