import {
  TREND_ASC, TREND_DES, TREND_EQL, RATES_TABLE_FILTERS,
} from '../../../globals';
import { clearString, } from '../../../utils/string';
import { comparatorCommon, } from '../../../utils/filter';


/**
 * Get trend
 */
const getNewTrend = (oldRate, newRate) => {
  if (!oldRate) return null;
  if (oldRate < newRate) return TREND_ASC;
  if (oldRate > newRate) return TREND_DES;
  return TREND_EQL;
};


/**
 * Parse Rates
 */
export const fulfilledRates = (ratesState, newRates) => {
  const ret = { ...ratesState, };

  const keys = Object.keys(newRates);
  for (let i = 0; i < keys.length; i++) {
    const id = keys[i];
    const newRate = newRates[id];

    let oldRate = null;
    if (Object.prototype.hasOwnProperty.call(ratesState, id)) oldRate = ratesState[id].rate;
    const newTrend = getNewTrend(
      oldRate,
      newRate
    );

    ret[id] = {
      rate: newRate,
      trend: newTrend,
    };
  }

  return ret;
};


/**
 * Parse Configuration data to rows
 */
export const parseConfiguration = (data) => {
  const ret = [];

  const keys = Object.keys(data);
  for (let i = 0; i < keys.length; i++) {
    const id = keys[i];
    const pair = data[id];
    const currencyL = pair[0];
    const currencyR = pair[1];

    const name = `${currencyL.code}/${currencyR.code}`;

    ret.push({
      id,
      name,
      nameClean: clearString(name),
    });
  }

  return ret;
};


/**
 * Apply filter to rows
 */
export const applyFilter = (rows, rates, filter) => {
  const { values, sort, } = filter;
  const filterNameClean = clearString(values.name);

  // filters
  const filtered = filterNameClean === ''
    ? [ ...rows, ]
    : rows.filter((row) => (row.nameClean.includes(filterNameClean)));

  // order
  if (!sort.name || !sort.order) return filtered;
  const sorted = filtered.sort((a, b) => {
    switch (sort.name) {
      // from unfiltered rows
      case RATES_TABLE_FILTERS.NAME: {
        return comparatorCommon(a.nameClean, b.nameClean, sort.order);
      }

      // from rates
      case RATES_TABLE_FILTERS.TREND:
      case RATES_TABLE_FILTERS.RATE: {
        const tmpAVal = Object.prototype.hasOwnProperty.call(rates, a.id)
          ? rates[a.id][sort.name]
          : null;
        const tmpBVal = Object.prototype.hasOwnProperty.call(rates, b.id)
          ? rates[b.id][sort.name]
          : null;
        return comparatorCommon(tmpAVal, tmpBVal, sort.order);
      }

      default: return 0;
    }
  });

  return sorted;
};
