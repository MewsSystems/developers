import { always, compose, cond, gt, lt, path, prop, reduceRight, T, toPairs } from 'ramda';
import { growthTrend } from '../../constants';
import { getCurrencyRate } from '../../selectors';
import { calculateGrowthRate, mergePath } from '../../utils';

const { DECLINING, STAGNATING, GROWING } = growthTrend;

const getNextTrend = (newRate) => cond([
    [gt(newRate), always(GROWING)],
    [lt(newRate), always(DECLINING)],
    [T, always(STAGNATING)],
]);

const prepareNewData = (currencyPairId, newRate, accumulator) => {
    const previousRate = getCurrencyRate(currencyPairId)(accumulator);
    const growthRate = calculateGrowthRate(previousRate, newRate);
    return ({ previousRate, currentRate: newRate, growthRate, trend: getNextTrend(newRate)(previousRate) });
};

const mergeRateToCurrencyPair = ([currencyPairId, newRate], accumulator) =>
    mergePath(['currencyPairs', 'byId', currencyPairId], prepareNewData(currencyPairId, newRate, accumulator), accumulator);

export default (state) =>
    compose(path(['currencyPairs', 'byId']), reduceRight(mergeRateToCurrencyPair, state), toPairs, prop('rates'));
