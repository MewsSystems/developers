import { assoc, contains, compose, pluck, reduceRight, slice } from 'ramda';

const isCurrencyPairIdToToggle = ({ id }, currentPage, maxNumberOfRows) => compose(
    contains(id),
    slice(maxNumberOfRows * (currentPage - 1), maxNumberOfRows * currentPage),
    pluck('id'),
);

const toggleTracking = (orderedCurrencyPairs, currentPage, maxNumberOfRows, track) => (currencyPair, accumulator) =>
    isCurrencyPairIdToToggle(currencyPair, currentPage, maxNumberOfRows)(orderedCurrencyPairs) ?
        { ...accumulator, [currencyPair.id]: assoc('track', track, currencyPair) } :
        { ...accumulator, [currencyPair.id]: currencyPair } ;

export default (orderedCurrencyPairs, currentPage, maxNumberOfRows, track) => ({
    type: 'COLLECTIVE_TOGGLE_TRACKING',
    payload: reduceRight(
        toggleTracking(orderedCurrencyPairs, currentPage, maxNumberOfRows, track),
        {},
        orderedCurrencyPairs
    ),
});
