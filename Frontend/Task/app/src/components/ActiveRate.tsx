import React from "react";
import CurrencyPair from "../models/Pair";
import { CurrencyRates } from "./CurrencyRates";
import Trend from "../models/Trend";

const ActiveRate: React.FC<{
  id: string;
  currencyPair: CurrencyPair;
  toggleVisibility: (s: string) => void;
}> = ({ id, currencyPair, toggleVisibility }) => {
  var trend = (tr: Trend) => {
    switch (tr) {
      case Trend.FALLING:
        return "F";

      case Trend.RAISING:
        return "R";
      case Trend.STABLE:
        return "S";
    }
  };
  return (
    <div>
      <button onClick={() => toggleVisibility(id)}>X</button>
      {currencyPair.currencies[0].code}/{currencyPair.currencies[1].code} -
      {currencyPair.rate} -{trend(currencyPair.trend)}
    </div>
  );
};

export default ActiveRate;
