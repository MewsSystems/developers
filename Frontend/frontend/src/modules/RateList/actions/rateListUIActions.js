export const RATE_LIST__CHANGE_FILTER_VALUE = 'RATE_LIST__CHANGE_FILTER_VALUE';
export const RATE_LIST__CHANGE_FILTER_SORT = 'RATE_LIST__CHANGE_FILTER_SORT';


/**
 * Change Filter Value
 * @param {String} name
 * @param {Any} value
 */
export const changeFilterValueAction = (name, value) => ({
  type: RATE_LIST__CHANGE_FILTER_VALUE,
  payload: {
    name,
    value,
  },
});


/**
 * Change Filter Sort Value
 * @param {String} name
 * @param {String} order
 */
export const changeFilterSortAction = (name, order) => ({
  type: RATE_LIST__CHANGE_FILTER_SORT,
  payload: {
    name,
    order,
  },
});
