import styles from './Main.css';

import React from "react";

import { CurrencyPairsSelector } from "./CurrencyPairsSelector";
import { CurrencyPairsRateList } from "./CurrencyPairsRateList";

export function Main(state) {
    return state.pairs.length ? (
        <div className={styles.main}>
            <div className={styles['left-container']}>
            {CurrencyPairsSelector(state.pairs)}
            </div>
            <div className={styles['right-container']}>
            {CurrencyPairsRateList(state)}
            </div>
        </div>
    ) : <h1>{state.message}</h1>;
}
