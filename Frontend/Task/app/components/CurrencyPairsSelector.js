import React from "react";

import styles from "./CurrencyPairsSelector.css"
import { CurrencyPairSelector } from "./CurrencyPairSelector";

export const CurrencyPairsSelector = (pairs) => {
    const pairsEls = pairs.map(pair => CurrencyPairSelector(pair));
    return <ul className={styles.list}>{pairsEls}</ul>;
};
