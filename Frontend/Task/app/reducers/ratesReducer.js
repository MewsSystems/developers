import {
  GET_RATES_STARTED,
  GET_RATES_SUCCESS,
  GET_RATES_FAILURE,
  COMPUTE_TRENDS
} from 'Actions/types';

const initialState = {
  loading: false,
  rates: {},
  trends: {},
  error: null
};

const ratesReducer = (state = initialState, action) => {
  switch (action.type) {
    case GET_RATES_STARTED:
      return {
        ...state,
        loading: true
      };

    case GET_RATES_SUCCESS:
      return {
        ...state,
        loading: false,
        error: null,
        rates: action.payload
      };

    case GET_RATES_FAILURE:
      return {
        ...state,
        loading: false,
        error: action.payload.error
      };

    case COMPUTE_TRENDS:
      return {
        ...state,
        trends: action.payload
      };

    default:
      return state;
  }
};

export default ratesReducer;
