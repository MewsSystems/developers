import React from "react";
import CurrencyPair from "../models/Pair";
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
      <button onClick={() => toggleVisibility(id)}>x</button>
      <div>
        {currencyPair.currencies[0].code}/{currencyPair.currencies[1].code}
      </div>
      <div className={trend(currencyPair.trend)}>{currencyPair.rate}</div>
    </div>
  );
};

export default ActiveRate;
