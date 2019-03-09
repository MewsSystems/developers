import React from 'react';
import Select from 'react-select';

import styles from './CurrencySelect.module.css';

const CurrencySelect = ({ currencyPairs, handleChange, value }) => {
  const pairToLabel = currencyPair => {
    const [from, to] = currencyPair;
    return `${from.code}/${to.code}`;
  };

  const pairsToOptions = currencyPairs => {
    return Object.entries(currencyPairs).map(([key, value]) => ({
      label: pairToLabel(value),
      value: key,
    }));
  };

  return (
    <Select
      className={styles.currencySelect}
      isMulti
      value={value}
      onChange={handleChange}
      options={pairsToOptions(currencyPairs)}
    />
  );
};

export default CurrencySelect;
