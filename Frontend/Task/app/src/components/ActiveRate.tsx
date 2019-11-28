import React from "react";
import CurrencyPair from "../models/Pair";
import { CurrencyRates } from "./CurrencyRates";
import Trend from "../models/Trend";

const ActiveRate: React.FC<{
  id: string;
  currencyPair: CurrencyPair;
  toggleVisibility: (s: string) => void;
}> = ({ id, currencyPair, toggleVisibility }) => {
  var trend = (tr: Trend): string => {
    switch (tr) {
      case Trend.FALLING:
        return "falling";

      case Trend.RAISING:
        return "rising";
      case Trend.STABLE:
        return "stable";
    }
  };
  return (
    <div className="active">
      <button onClick={() => toggleVisibility(id)}>X</button>
      <p className={trend(currencyPair.trend)}>
        {currencyPair.currencies[0].code}/{currencyPair.currencies[1].code}
      </p>
      <p>{currencyPair.rate}</p>
    </div>
  );
};

export default ActiveRate;
