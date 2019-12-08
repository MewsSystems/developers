import React from "react";
import { CurrencyPairRate } from "./CurrencyPairRate";

import styles from "./CurrencyPairsRateList.css"

export const CurrencyPairsRateList = ({selected, message}) => {
    const rateEls = selected.map(rate => CurrencyPairRate(rate));
    return (
        <table className={styles.container}>
            <thead className={styles.header}>
                <tr>
                    <th colSpan="3">
                        <span>{message}</span>
                    </th>
                </tr>
            </thead>
            <tbody>
            {rateEls}
            </tbody>
        </table>
    );
};
