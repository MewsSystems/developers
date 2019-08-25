import { createSelector } from 'reselect';
import { mergeDeepWith, concat, or, isNil } from 'ramda';
import { transformPairs, transformRates, transformTrends } from 'Utils';
import { getPairs } from 'Selectors/currencyPairsSelector';
import { getRatesData, getTrends } from 'Selectors/currencyRatesSelector';

export const getTableData = createSelector(
  [getPairs, getRatesData, getTrends],
  (pairs, rates, trends) => {
    if (or(isNil(pairs), isNil(rates))) return {};
    const transformedPairs = transformPairs(pairs);
    const transformedRates = transformRates(rates);
    const transformedTrends = transformTrends(trends);
    return mergeDeepWith(
      concat,
      transformedTrends,
      mergeDeepWith(concat, transformedRates, transformedPairs)
    );
  }
);
