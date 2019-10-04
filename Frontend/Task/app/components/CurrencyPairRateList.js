import React from 'react';
import PropTypes from 'prop-types';

import CurrencyPairRateItem from './CurrencyPairRateItem';
import CurrencyPairRateStatus from './CurrencyPairRateStatus';
import styles from './CurrencyPairRateList.module.css';

// The CurrencyPairRateList component renders a list of selected currency pairs
const CurrencyPairRateList = ({
  errorFetchingRatesCount,
  errorFetchingRates,
  errorIsPersistent,
  isFetchingRates,
  ratesToDisplay,
  selectedCurrencyPairs,
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
  const validRatesCount = ratesToDisplay.filter(rate => rate.currentValue).length;
  const ratesAreAvailable = validRatesCount > 0
    && (validRatesCount === selectedCurrencyPairs.length);

  return (
    <div className={styles.wrapper}>
      {(errorIsPersistent)
        ? ''
        : (
          <ul className={styles.list}>
            {currencyRatesArray}
          </ul>
        )}
      <CurrencyPairRateStatus
        errorFetchingRatesCount={errorFetchingRatesCount}
        errorFetchingRates={errorFetchingRates}
        errorIsPersistent={errorIsPersistent}
        isFetchingRates={isFetchingRates}
        ratesAreAvailable={ratesAreAvailable}
        selectedCurrencyPairsCount={selectedCurrencyPairs.length}
      />
    </div>
  );
};

CurrencyPairRateList.propTypes = {
  errorFetchingRatesCount: PropTypes.number.isRequired,
  errorFetchingRates: PropTypes.string.isRequired,
  errorIsPersistent: PropTypes.bool.isRequired,
  isFetchingRates: PropTypes.bool.isRequired,
  ratesToDisplay: PropTypes.arrayOf(PropTypes.object).isRequired,
  selectedCurrencyPairs: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default CurrencyPairRateList;
