import * as actionTypes from './actionTypes';

export const filter = item => {
  return {
    type: actionTypes.FILTER,
    payload: item
  };
};

export const resetFilter = () => {
  return {
    type: actionTypes.RESET_FILTER
  };
};
