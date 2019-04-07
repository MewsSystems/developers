import { createSelector } from 'reselect';

export const getCurrencyPairs = state => state.configuration.currencyPairs || {};

/**
 * Get selected pairs from store
 */
const getSelectedPairs = state => state.rates.selected;

/**
 * Get current rates from store
 */
const getCurrentRates = state => state.rates.current;

/**
 * Get previous rates from store
 */
const getPreviousRates = state => state.rates.previous;

const getCurrenciesList = createSelector(
  [getSelectedPairs, getCurrentRates, getPreviousRates],
  (selected, current, previous) => selected.map(({ label, value }) => ({
    label,
    value,
    next: current[value] ? current[value] : null,
    prev: previous[value] ? previous[value] : null,
  })),
);

export default getCurrenciesList;
