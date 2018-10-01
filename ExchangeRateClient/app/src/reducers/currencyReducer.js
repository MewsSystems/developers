import {
  GET_CONFIG,
  GET_RATES,
  SPINNER_LOADING,
} from '../actions/types';

const initialState = {
  configuration: [],
  loading: false,
  rateKey:"",
  rates: [],
};

export default function (state = initialState, action) {
  switch (action.type) {
    case GET_CONFIG:
      return {
        ...state,
        configuration: action.payload,
        loading: false,
        error: false
      };
    case GET_RATES:
      return {
        ...state,
        rates: action.payload,
        rateKey:action.rateKey,
        loading: false,
      };

    default:
      return state;
  }
}

