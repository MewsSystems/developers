// @flow
import { UPDATE_TRENDS } from '../constants';

const rates = (state: Object = {}, data: Object) => {
  const { exchangeID, exchangeRate } = data;
  const existing = state[exchangeID] || [];
  // Keep the trend array from growing too large
  existing.splice(0, existing.length - 20);

  switch (data.type) {
    case UPDATE_TRENDS:
      return {
        ...state,
        [exchangeID]: [...existing, exchangeRate],
      };
    default:
      return state;
  }
};

export default rates;
