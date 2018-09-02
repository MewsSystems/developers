import { createSelector } from 'reselect';
import { apply, compose, defaultTo, indexOf, nth, pluck, prop } from 'ramda';

export const getRatesHistory = prop('ratesHistory');

export const getRateHistoryEntries = (currencyPairId) => createSelector(
    [getRatesHistory], compose(defaultTo([]), prop(currencyPairId)),
);

export const getHighestRateFromCurrencyPair = (currencyPairId) => createSelector(
    [getRateHistoryEntries(currencyPairId)], (rateHistoryEntries) => {
        const allRates = pluck('rate', rateHistoryEntries);
        const index = indexOf(apply(Math.max, allRates), allRates);
        return defaultTo('No data', nth(index, rateHistoryEntries));
    }
);

export const getLowestRateFromCurrencyPair = (currencyPairId) => createSelector(
    [getRateHistoryEntries(currencyPairId)], (rateHistoryEntries) => {
        const allRates = pluck('rate', rateHistoryEntries);
        const index = indexOf(apply(Math.min, allRates), allRates);
        return defaultTo('No data', nth(index, rateHistoryEntries));
    }
);
