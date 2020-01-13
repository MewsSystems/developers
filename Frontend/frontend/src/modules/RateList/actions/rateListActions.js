export const RATE_LIST__CHANGE_FILTER_VALUE = 'RATE_LIST__CHANGE_FILTER_VALUE';


/**
 * Change Filter Value
 */
export const changeFilterValueAction = (newFilter) => ({
  type: RATE_LIST__CHANGE_FILTER_VALUE,
  payload: newFilter,
});
