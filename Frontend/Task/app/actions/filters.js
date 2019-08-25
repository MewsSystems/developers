import { cond, T, always, isNil, prop } from 'ramda';
import { SET_FILTER_PARAMS } from 'Actions/types';
import { getFilterParams } from 'Selectors';

const defineOrder = cond([
  [isNil, always('desc')],
  [T, sortParams => prop('order', sortParams)]
]);

export const setFilterParams = sortKey => {
  return (dispatch, getState) => {
    const sortParams = getFilterParams(getState());
    const order = defineOrder(sortParams);
    
    dispatch({
      type: SET_FILTER_PARAMS,
      payload: {
        key: sortKey,
        order: order === 'desc' ? 'asc' : 'desc'
      }
    });
  }
};
