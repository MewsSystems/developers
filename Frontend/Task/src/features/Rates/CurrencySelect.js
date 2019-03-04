import React from 'react';
import Select from 'react-select';

import styles from './CurrencySelect.module.css';

const CurrencySelect = ({ handleChange, options, value }) => {
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
