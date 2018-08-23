// @flow
import { STORE_PAIRS } from '../constants';

const pairs = (state: Object = {}, action: Object) => {
  switch (action.type) {
    case STORE_PAIRS:
      return {
        ...state,
        ...action.pairs.currencyPairs,
      };
    default:
      return state;
  }
};

export default pairs;
