import React from "react";
import CurrencyPair from "../models/Pair";
import styles from "../style.module.css";

const PassiveRate: React.FC<{
  id: string;
  currencyPair: CurrencyPair;
  toggleVisibility: (id: string) => void;
}> = ({ id, currencyPair, toggleVisibility }) => {
  return (
    <div className={styles.passive}>
      <button onClick={() => toggleVisibility(id)}>+</button>
      {currencyPair.currencies[0].code} / {currencyPair.currencies[1].code}{" "}
    </div>
  );
};

export default PassiveRate;
