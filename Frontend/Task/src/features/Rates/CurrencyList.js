import React from 'react';

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
    <ul>
      {currencyList.map(({ from, value, to, trend }) => {
        return (
          <li key={value}>{`${from.code}/${to.code} ${getTrendSymbol(
            trend,
          )}`}</li>
        );
      })}
    </ul>
  );
};

export default CurrencyList;
