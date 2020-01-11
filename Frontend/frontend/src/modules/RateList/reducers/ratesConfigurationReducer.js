import {
  DATA__GET_RATES_CONFIGURATION,
} from '../actions/ratesConfigurationActions';
import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';


const initialState = {
  loading: true,
  data: null,
  error: null,
  timestamp: null,
};


/**
 * Rates Configuration Reducer
 * @param {Object} state
 * @param {Object} action
 */
const reducer = (state = initialState, action) => {
  const { type, payload, } = action;
  switch (type) {
    case `${DATA__GET_RATES_CONFIGURATION}__${PENDING}`: {
      return {
        ...state,
        loading: true,
      };
    }

    case `${DATA__GET_RATES_CONFIGURATION}__${FULFILLED}`: {
      return {
        ...state,
        loading: false,
        data: payload.currencyPairs,
        error: null,
        timestamp: Date.parse(new Date()),
      };
    }

    case `${DATA__GET_RATES_CONFIGURATION}__${REJECTED}`: {
      return {
        ...state,
        loading: false,
        error: payload,
      };
    }

    default:
      return state;
  }
};

export default reducer;
