import React from "react";

import styles from "./CurrencyPairRate.css";

export const CurrencyPairRate = pair => {
    return (
        <tr key={pair.id} className={styles.listitem}>
            <td >
                <span>{pair.trend}</span>
            </td>
            <td >
                <div>
                    <span>{pair.frname}</span>
                </div>
                <div>
                    <span>{pair.toname}</span>
                </div>
            </td>
            <td >
                <span>{pair.rate}</span>
            </td>
        </tr>
    );
};
