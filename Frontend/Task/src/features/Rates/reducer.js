import { FETCH_CONFIGURATION, FETCH_RATES, SET_RATES } from './actions';

export const start = type => `${type}_PENDING`;
export const success = type => `${type}_FULFILLED`;
export const error = type => `${type}_REJECTED`;

const initialState = {
  currencyPairs: {},
  current: {},
  fetchRatesId: null,
  isLoadingConfig: false,
  isLoadingRates: false,
  lastUpdate: null,
  previous: {},
  selected: [],
};

export default function(state = initialState, action) {
  const { meta, payload, type } = action;

  switch (type) {
    case start(FETCH_CONFIGURATION):
      return {
        ...state,
        isLoadingConfig: true,
      };

    case success(FETCH_CONFIGURATION):
      return {
        ...state,
        currencyPairs: payload.currencyPairs ? payload.currencyPairs : {},
        isLoadingConfig: false,
      };

    case error(FETCH_CONFIGURATION):
      return {
        ...state,
        isLoadingConfig: false,
      };

    case start(FETCH_RATES):
      return {
        ...state,
        fetchRatesId: meta.fetchRatesId,
        isLoadingRates: true,
      };

    case success(FETCH_RATES):
      return meta.fetchRatesId === state.fetchRatesId
        ? {
            ...state,
            current: payload.rates ? payload.rates : state.current,
            isLoadingRates: false,
            lastUpdate: new Date().toISOString(),
            previous: payload.rates ? state.current : state.previous,
          }
        : state;

    case error(FETCH_RATES):
      return meta.fetchRatesId === state.fetchRatesId
        ? {
            ...state,
            isLoadingRates: false,
          }
        : state;

    case SET_RATES:
      return {
        ...state,
        selected: payload,
      };

    default:
      return state;
  }
}
