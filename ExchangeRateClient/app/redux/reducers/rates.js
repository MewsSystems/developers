// @flow
import GET_RATES from '../constants';

const Rates = (state: Object = {}, action: Object) => {
  switch (action.type) {
    case GET_RATES:
      return {
        ...state,
        rates: {},
      };
    default:
      return state;
  }
};

export default Rates;
