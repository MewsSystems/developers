import { RATES } from '../constants/actionTypes';

const initialState = {
  isLoading: false,
  isError: false,
  selected: [],
  current: {},
  previous: {},
};

/**
 * Configuration reducer
 */
export default function (state = initialState, action) {
  switch (action.type) {
    case `REQUEST_${RATES}`: {
      return {
        ...state,
        isLoading: true,
        isError: false,
      };
    }
    case `SUCCESS_${RATES}`: {
      return {
        ...state,
        current: action.payload.rates || state.current,
        previous: action.payload.rates ? state.current : state.previous,
        isLoading: false,
        isError: false,
      };
    }
    case `ERROR_${RATES}`: {
      return {
        ...state,
        isLoading: false,
        isError: true,
      };
    }
    case `SET_${RATES}`: {
      return {
        ...state,
        selected: action.payload,
      };
    }
    default:
      return state;
  }
}
