// @flow
import { STORE_PAIRS } from '../constants';

const pairs = (state: Object = {}, action: Object) => {
  const { type, pairData } = action;
  switch (type) {
    case STORE_PAIRS:
      // Add pairs to local storage
      localStorage.setItem('xChangePairsMews', JSON.stringify(pairData));
      return {
        ...state,
        ...pairData.currencyPairs,
      };
    default:
      return state;
  }
};

export default pairs;
