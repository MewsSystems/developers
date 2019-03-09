import React from 'react';
import Select from 'react-select';

import styles from './CurrencySelect.module.css';

const CurrencySelect = ({ currencyPairs, handleChange, value }) => {
  const pairToLabel = currencyPair => {
    const [from, to] = currencyPair;
    return `${from.code}/${to.code}`;
  };

  const options = Object.entries(currencyPairs).map(([key, keyValue]) => ({
    label: pairToLabel(keyValue),
    value: key,
  }));

  return (
    <Select
      className={styles.currencySelect}
      isMulti
      value={value}
      onChange={handleChange}
      options={options}
    />
  );
};

export default CurrencySelect;
