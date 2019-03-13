import React from 'react';
import Select from 'react-select';

import styles from './CurrencySelect.module.css';

const pairToLabel = currencyPair => {
  const [from, to] = currencyPair;
  return `${from.code}/${to.code}`;
};

const CurrencySelect = ({ currencyPairs, handleChange, isDisabled, value }) => {
  const options = Object.entries(currencyPairs).map(([key, keyValue]) => {
    return {
      label: pairToLabel(keyValue),
      value: key,
    };
  });

  return (
    <Select
      className={styles.currencySelect}
      isMulti
      isDisabled={isDisabled}
      value={value}
      onChange={handleChange}
      options={options}
    />
  );
};

export default CurrencySelect;
