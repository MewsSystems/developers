import { createSelector } from 'reselect';
import { assoc, compose, filter, keys, map, pluck, prop, propEq } from 'ramda';
import { objSize } from '../utils';

const getCurrencyPairs = prop('currencyPairs');

export const getCurrencyPairsById = createSelector([getCurrencyPairs], prop('byId'));

export const getNumberOfCurrencyPairs = createSelector([getCurrencyPairsById], objSize);

export const getCurrencyPairIds = createSelector([getCurrencyPairsById], keys);

export const getTrackedCurrencyPairIds =
    createSelector([getCurrencyPairsById], compose(keys, filter(propEq('track', true))));

export const getCurrencyPair = (currencyPairId) => createSelector([getCurrencyPairsById], prop(currencyPairId));

export const getCurrencyRate = (currencyPairId) =>
    createSelector([getCurrencyPair(currencyPairId)], prop('currentRate'));

export const getCurrencyPairsAllIds = createSelector([getCurrencyPairs], prop('allIds'));

export const getOrderedCurrencyPairs = createSelector(
    [getCurrencyPairsById, getCurrencyPairsAllIds],
    (currencyPairsById, currencyPairsAllIds) => map(
        (currencyPairId) => assoc('id', currencyPairId, prop(currencyPairId, currencyPairsById)),
        currencyPairsAllIds
    )
);

export const getOrderedCurrencyPairsIds = createSelector([getOrderedCurrencyPairs], pluck('id'));
