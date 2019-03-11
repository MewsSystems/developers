import React from 'react';

import styles from './CurrencyList.module.css';

const UP = 'up';
const DOWN = 'down';
const EQUAL = 'equal';
const UNKNOWN = 'unknown';

const getTrend = (prevValue, nextValue) => {
  if (!prevValue || !nextValue) {
    return UNKNOWN;
  }
  if (prevValue < nextValue) {
    return UP;
  } else if (prevValue > nextValue) {
    return DOWN;
  } else if (prevValue === nextValue) {
    return EQUAL;
  }
  return UNKNOWN;
};

const trendSymbols = {
  [UP]: '▲',
  [DOWN]: '▼',
  [EQUAL]: '-',
  [UNKNOWN]: '?',
};

const CurrencyList = ({ currencyList }) => {
  return (
    <table className={styles.currencyList}>
      <thead>
        <tr>
          <th>From/To</th>
          <th>Value</th>
          <th>Trend</th>
        </tr>
      </thead>
      <tbody>
        {currencyList.map(({ curValue, label, prevValue, value }) => {
          const trend = getTrend(prevValue, curValue);
          return (
            <tr key={value}>
              <td>{label}</td>
              <td>{curValue}</td>
              <td className={styles[trend]}>{trendSymbols[trend]}</td>
            </tr>
          );
        })}
      </tbody>
    </table>
  );
};

export default CurrencyList;
