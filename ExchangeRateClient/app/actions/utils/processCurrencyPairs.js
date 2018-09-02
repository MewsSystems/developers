import { assoc, compose, prop, reduceRight, toPairs } from 'ramda';

const assocCurrencyPair = ([currencyPairId, currencyPair], accumulator) =>
    assoc(currencyPairId, { pair: currencyPair, track: true }, accumulator);

export default compose(
    reduceRight(assocCurrencyPair, {}),
    toPairs,
    prop('currencyPairs'),
);
