import { FETCH_CURRENCIES } from '../actions/types';

export default (state = {}, action) => {
  switch (action.type) {
    case FETCH_CURRENCIES:
      return { ...state, ...action.payload };
    default:
      return state;
  }
};
