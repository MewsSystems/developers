import React from 'react';
import PropTypes from 'prop-types';

import CurrencyPair from './CurrencyPair';
import styles from './CurrencyPairSelector.module.css';

// The CurrencyPairSelector component renders a list of available currency pairs
// to be filtered and/or selected, either individually or en masse
const CurrencyPairSelector = ({
  currencyPairs,
  deselectAllButtonDisabled,
  deselectAllVisibleCurrencyPairs,
  errorFetchingConfiguration,
  filter,
  isFetchingConfiguration,
  selectAllButtonDisabled,
  selectAllVisibleCurrencyPairs,
  selectedCurrencyPairs,
  handleSetCurrencyFilter,
  toggleCurrencyPairSelection,
}) => {
  if (currencyPairs.length === 0 && isFetchingConfiguration) { // render loading placeholder
    return (
      <div className={styles.wrapper}>
        <p className={styles.loading}>Loading currency pair list, please wait.</p>
      </div>
    );
  }
  if (currencyPairs.length === 0 && errorFetchingConfiguration) {
    // render error message if API failure and nothing from localStorage
    return (
      <div className={styles.wrapper}>
        <div className={styles.error}>
          <p>
            Sorry, there was a problem loading the currency pair list.
            Please refresh the page to try again.
          </p>
          <p>
            {`The error received was "${errorFetchingConfiguration}".`}
          </p>
        </div>
      </div>
    );
  }
  const currencyPairArray = Object.keys(currencyPairs).map((currencyPairId) => {
    const isSelected = selectedCurrencyPairs.includes(currencyPairId);
    return (
      <li key={currencyPairId}>
        <CurrencyPair
          currencyPairId={currencyPairId}
          currencyPair={currencyPairs[currencyPairId]}
          isSelected={isSelected}
          toggleCurrencyPairSelection={toggleCurrencyPairSelection}
        />
      </li>
    );
  });

  return (
    <div className={styles.wrapper}>
      <p className={styles.header}>
        <input
          type="search"
          placeholder="Search for currencies by name or code"
          onChange={handleSetCurrencyFilter}
          value={filter}
          className={styles.filter}
        />
        <button
          type="button"
          disabled={selectAllButtonDisabled}
          onClick={selectAllVisibleCurrencyPairs}
          className={styles.selectionButton}
        >
          Select all shown
        </button>
        <button
          type="button"
          disabled={deselectAllButtonDisabled}
          onClick={deselectAllVisibleCurrencyPairs}
          className={styles.selectionButton}
        >
          Deselect all shown
        </button>
      </p>
      {(currencyPairs.length > 0 && currencyPairArray.length === 0)
        ? <p className={styles.noMatch}>Sorry, no currency pairs match your search criteria.</p>
        : (
          <ul className={styles.currencyPairList}>
            {currencyPairArray}
          </ul>
        )}
    </div>
  );
};

CurrencyPairSelector.propTypes = {
  currencyPairs: PropTypes.objectOf(PropTypes.array).isRequired,
  deselectAllButtonDisabled: PropTypes.bool.isRequired,
  deselectAllVisibleCurrencyPairs: PropTypes.func.isRequired,
  filter: PropTypes.string.isRequired,
  errorFetchingConfiguration: PropTypes.string.isRequired,
  isFetchingConfiguration: PropTypes.bool.isRequired,
  selectAllButtonDisabled: PropTypes.bool.isRequired,
  selectAllVisibleCurrencyPairs: PropTypes.func.isRequired,
  selectedCurrencyPairs: PropTypes.arrayOf(PropTypes.string).isRequired,
  handleSetCurrencyFilter: PropTypes.func.isRequired,
  toggleCurrencyPairSelection: PropTypes.func.isRequired,
};

export default CurrencyPairSelector;
