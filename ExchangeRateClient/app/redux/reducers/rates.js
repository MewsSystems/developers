// @flow
import { UPDATE_RATES } from '../constants';

const rates = (state: Object = {}, action: Object) => {
  switch (action.type) {
    case UPDATE_RATES:
      return {
        ...state,
        ...action.rateData.rates,
      };
    default:
      return state;
  }
};

export default rates;
