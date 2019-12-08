import React from "react";
import { toggleCurrencyPair } from "../sam/Actions";

import styles from "./CurrencyPairSelector.css";

export const CurrencyPairSelector = pair => {
    return (
        <li
            className={`${styles.listitem} ${pair.selected ? styles.selected : styles.unselected}`}
            key={pair.id}
            onClick={() => toggleCurrencyPair(pair.id)}
        >
            <div className={styles["listitem-content"]}>
                <span>
                    {pair.frcode}/{pair.tocode}
                </span>
            </div>
        </li>
    );
};
