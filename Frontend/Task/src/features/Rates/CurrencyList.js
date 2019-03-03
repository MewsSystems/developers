import React from 'react';

const CurrencyList = ({ currencyList }) => {
  return (
    <ul>
      {currencyList.map(({ from, value, to, trend }) => {
        return <li key={value}>{`${from.code}/${to.code}/${trend}`}</li>;
      })}
    </ul>
  );
};

export default CurrencyList;
