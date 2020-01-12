import {
  SORT_ASC, SORT_DES, SORT_UNSET,
} from '../globals';


/**
 * Compare functions for sorting
 */
const SORT_COMPARATORS = {
  [SORT_ASC]: (a, b) => ((a < b) ? -1 : ((a > b) ? 1 : 0)), // eslint-disable-line no-nested-ternary
  [SORT_DES]: (a, b) => ((a > b) ? -1 : ((a < b) ? 1 : 0)), // eslint-disable-line no-nested-ternary
  [SORT_UNSET]: () => 0,
};


/**
 * Sort comparator
 * @param {OneOfType([ String, number, ])} a
 * @param {OneOfType([ String, number, ])} b
 * @param {String} order
 */
export const comparatorCommon = (a, b, order) => {
  if (!order) return 0;

  if (Object.prototype.hasOwnProperty.call(SORT_COMPARATORS, order)) {
    return SORT_COMPARATORS[order](a, b);
  }
  return 0;
};
