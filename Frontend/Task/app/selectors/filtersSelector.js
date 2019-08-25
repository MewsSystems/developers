import { createSelector } from 'reselect';
import { prop, toPairs, fromPairs } from 'ramda';
import { getTableData } from 'Selectors/dataSelector';
import { sortData } from 'Utils';

const filters = prop('filters');

export const getFilterParams = createSelector(
  [filters],
  prop('sortParams')
);

export const getSortedTableData = createSelector(
  [getTableData, getFilterParams],
  (data, filter) => {
    if (filter) {
      const key = prop('key', filter);
      const order = prop('order', filter);
      return fromPairs(sortData(key, order)(toPairs(data)));
    }
    return data;
  }
);
