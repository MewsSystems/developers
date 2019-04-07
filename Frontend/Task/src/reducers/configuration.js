import { CONFIGURATION } from '../constants/actionTypes';

const initialState = {
  isLoading: false,
  isError: false,
};

/**
 * Configuration reducer
 */
export default function (state = initialState, action) {
  switch (action.type) {
    case `REQUEST_${CONFIGURATION}`: {
      return {
        ...state,
        isLoading: true,
        isError: false,
      };
    }
    case `SUCCESS_${CONFIGURATION}`: {
      return {
        ...state,
        ...action.payload,
        isLoading: false,
        isError: false,
      };
    }
    case `ERROR_${CONFIGURATION}`: {
      return {
        ...state,
        isLoading: false,
        isError: true,
      };
    }
    default:
      return state;
  }
}
