import React from 'react';

import styles from './CurrencyList.module.css';

export const TREND_UP = 'up';
export const TREND_DOWN = 'down';
export const TREND_EQUAL = 'equal';
export const TREND_UNKNOWN = 'unknown';

const CurrencyList = ({ currencyList }) => {
  const getTrendSymbol = trend => {
    switch (trend) {
      case TREND_UP:
        return '⇧';
      case TREND_DOWN:
        return '⇩';
      case TREND_EQUAL:
        return '=';
      default:
        return '?';
    }
  };

  return (
    <table className={styles.currencyList}>
      <thead>
        <tr>
          <th>From</th>
          <th>To</th>
          <th>Trend</th>
        </tr>
      </thead>
      <tbody>
        {currencyList.map(({ from, value, to, trend }) => {
          return (
            <tr key={value}>
              <td>{from.code}</td>
              <td>{to.code}</td>
              <td className={styles[trend]}>{getTrendSymbol(trend)}</td>
            </tr>
          );
        })}
      </tbody>
    </table>
  );
};

export default CurrencyList;
