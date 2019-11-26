import React from "react";
import CurrencyPair from "../models/Pair";

const PassiveRate: React.FC<{
  id: string;
  currencyPair: CurrencyPair;
  toggleVisibility: (s: string) => void;
}> = ({ id, currencyPair, toggleVisibility }) => {
  return (
    <button onClick={() => toggleVisibility(id)}>
      {" "}
      {currencyPair.currencies[0].code} /{currencyPair.currencies[1].code}{" "}
    </button>
  );
};

export default PassiveRate;
