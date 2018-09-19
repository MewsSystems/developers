// @flow

import {
  CONFIG_FETCH_SUCCESS,
  CONFIGS_FETCH_START,
  CONFIGS_FETCH_FAIL,
  RATES_FETCH_START,
  RATES_FETCH_SUCCESS,
  RATES_FETCH_FAIL,
  SELECT_RATES_IDS,
  type Action,
} from "../actions/configActions";
import type { StateTypes } from "./types";

export const initalState: StateTypes = {
  selectedRates: [],
  data: {},
  isFetchingConfig: false,
  APIerror: null,
  APIerror: null,
  isFetchingRates: false,
  rates: {},
};

export default (state: StateTypes = { ...initalState }, action: Action) => {
  switch (action.type) {
    case CONFIG_FETCH_SUCCESS:
      return {
        ...state,
        isFetchingConfig: false,
        ...action.payload,
      };
    case CONFIGS_FETCH_START:
      return {
        ...state,
        isFetchingConfig: true,
      };
    case CONFIGS_FETCH_FAIL:
      return {
        ...initalState,
        APIerror: action.payload.error,
      };
    case RATES_FETCH_START:
      return {
        ...state,
        selectedRates: action.payload,
        isFetchingRates: true,
      };
    case RATES_FETCH_SUCCESS: {
      const rates = action.payload;
      const ids = Object.keys(rates);

      const temp = ids.reduce(
        (acc, id) => ({
          ...acc,
          [id]: {
            current: rates[id],
            before: state.rates && state.rates[id] && state.rates[id].current,
          },
        }),
        {},
      );

      return {
        ...state,
        rates: temp,
        isFetchingRates: false,
      };
    }
    case RATES_FETCH_FAIL:
      return {
        ...state,
        isFetchingRates: false,
        APIerror: action.payload.error,
      };

    case SELECT_RATES_IDS:
      return {
        ...state,
        selectedRates: action.payload.ids,
      };

    default:
      return state;
  }
};
