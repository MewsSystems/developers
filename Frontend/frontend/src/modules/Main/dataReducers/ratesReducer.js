import {
  DATA__GET_RATES,
} from '../dataActions/ratesActions';
import {
  PENDING, FULFILLED, REJECTED,
} from '../../../globals';


const initialState = {
  loading: false,
  data: null,
  error: null,
};


/**
 * Rates Reducer
 */
const reducer = (state = initialState, action) => {
  const { type, payload, } = action;
  switch (type) {
    case `${DATA__GET_RATES}__${PENDING}`: {
      return {
        ...state,
        loading: true,
      };
    }

    case `${DATA__GET_RATES}__${FULFILLED}`: {
      return {
        ...state,
        loading: false,
        data: payload.rates,
        error: null,
      };
    }

    case `${DATA__GET_RATES}__${REJECTED}`: {
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
