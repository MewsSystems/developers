import React from 'react';
import PropTypes from 'prop-types';

import CurrencyPairRateItem from './CurrencyPairRateItem';
import styles from './CurrencyPairRateList.module.css';

// The CurrencyPairRateList component renders a list of selected currency pairs
const CurrencyPairRateList = ({
  errorIsPersistent,
  ratesToDisplay,
}) => {
  const currencyRatesArray = ratesToDisplay.map((rate) => {
    return (
      <li key={rate.shortcutName}>
        <CurrencyPairRateItem
          rateToDisplay={rate}
        />
      </li>
    );
  });

  return (
    <div className={styles.wrapper}>
      {(errorIsPersistent)
        ? ''
        : (
          <ul className={styles.list}>
            {currencyRatesArray}
          </ul>
        )}
    </div>
  );
};

CurrencyPairRateList.propTypes = {
  errorIsPersistent: PropTypes.bool.isRequired,
  ratesToDisplay: PropTypes.arrayOf(PropTypes.object).isRequired,
};

export default CurrencyPairRateList;
