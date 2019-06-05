import { Action, AppState } from "./types";

const initialState: AppState = {
  rates: undefined,
  isFetching: false,
  currencyPairs: undefined,
  error: ""
};

export const rootReducer = (
  state: AppState = initialState,
  action: Action
): AppState => {
  switch (action.type) {
    case `FETCH_EXCHANGE_PENDING`: {
      return {
        ...state,
        isFetching: true,
        error: ""
      };
    }
    case `FETCH_EXCHANGE_FULFILLED`: {
      return {
        ...state,
        rates: action.payload.rates,
        isFetching: false,
        error: ""
      };
    }
    case `FETCH_EXCHANGE_REJECTED`: {
      return {
        ...state,
        isFetching: false,
        error: "FETCH_EXCHANGE_REJECTED"
      };
    }
    case `DOWNLOAD_CONFIG_PENDING`: {
      return {
        ...state,
        isFetching: true,
        error: ""
      };
    }
    case `DOWNLOAD_CONFIG_FULFILLED`: {
      return {
        ...state,
        currencyPairs: action.payload.currencyPairs,
        isFetching: false,
        error: ""
      };
    }
    case `DOWNLOAD_CONFIG_REJECTED`: {
      return {
        ...state,
        isFetching: false,
        error: "DOWNLOAD_CONFIG_REJECTED"
      };
    }
    default:
      return state;
  }
};
