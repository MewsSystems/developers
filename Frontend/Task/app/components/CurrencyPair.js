import React from 'react';
import PropTypes from 'prop-types';

import styles from './CurrencyPair.module.css';

// The CurrencyPair component renders an individual currency pair to be selected
const CurrencyPair = ({
  currencyPair,
  currencyPairId,
  isSelected,
  toggleCurrencyPairSelection,
}) => {
  const { code: code1, name: name1 } = currencyPair[0];
  const { code: code2, name: name2 } = currencyPair[1];
  const shortcutCode = `${code1}/${code2}`;
  const shortcutName = `${name1}/${name2}`;
  return (
    <div className={styles.wrapper}>
      <input
        type="checkbox"
        name={currencyPairId}
        checked={isSelected}
        onChange={toggleCurrencyPairSelection}
        className={styles.checkbox}
      />
      <span className={styles.names}>{shortcutName}</span>
      <span className={styles.codes}>{`(${shortcutCode})`}</span>
    </div>
  );
};

CurrencyPair.propTypes = {
  currencyPair: PropTypes.arrayOf(PropTypes.objectOf(PropTypes.string)).isRequired,
  currencyPairId: PropTypes.string.isRequired,
  isSelected: PropTypes.bool.isRequired,
  toggleCurrencyPairSelection: PropTypes.func.isRequired,
};

export default CurrencyPair;
