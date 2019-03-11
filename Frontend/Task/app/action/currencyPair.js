import {
  prop, map, compose, join,
} from 'ramda';

export const getCurrencyPairShortcut = currency => (
  compose(join('/'), map(prop('code')))(currency)
);

export const getCurrencyPairWithShortcut = currencyPair => (
  { shortcut: getCurrencyPairShortcut(currencyPair.pair), ...currencyPair }
);

export const getPairCollectionWithShortcut = pairs => (
  map(getCurrencyPairWithShortcut, pairs)
);

export const iteratePairCollection = (pairs, callback) => Object.keys(pairs).map(
  key => callback(key, pairs[key]),
);
