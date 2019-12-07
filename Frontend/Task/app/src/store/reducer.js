import {
  REQUEST_CONFIG,
  SAVE_CONFIG,
  CONFIG_ERROR,
  REQUEST_UPDATE,
  SAVE_UPDATE,
  UPDATE_ERROR,
  SET_FILTERS
} from "./constants";

const initialState = {
  currencyPairs: {},
  filteredPairs: [],
  loading: false,
  error: ""
};

export const reducer = (state = initialState, action) => {
  switch (action.type) {
    case REQUEST_CONFIG:
      return {
        ...state,
        loading: true
      };
    case SAVE_CONFIG:
      return {
        ...state,
        currencyPairs: action.currencyPairs,
        filteredPairs: action.filteredPairs,
        loading: false
      };
    case CONFIG_ERROR:
      return {
        ...state,
        error: action.error,
        loading: false
      };
    case REQUEST_UPDATE:
      return {
        ...state,
        error: ""
      };
    case SAVE_UPDATE:
      return {
        ...state,
        currencyPairs: action.currencyPairs
      };
    case UPDATE_ERROR:
      return {
        ...state,
        error: action.error
      };
    case SET_FILTERS:
      return {
        ...state,
        filteredPairs: action.filteredPairs
      };
    default:
      return state;
  }
};
