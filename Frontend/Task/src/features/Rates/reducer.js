import { FETCH_CONFIGURATION, FETCH_RATES, SET_RATES } from './actions';

export const startT = type => `${type}_PENDING`;
export const successT = type => `${type}_FULFILLED`;
export const errorT = type => `${type}_REJECTED`;

const initialState = {
  currencyPairs: {},
  previous: {},
  current: {},
  selected: [],
  isLoadingConfig: false,
  isLoadingRates: false,
  lastUpdate: null,
};

export default function(state = initialState, action) {
  const { payload, type } = action;

  switch (type) {
    case startT(FETCH_CONFIGURATION):
      return {
        ...state,
        isLoadingConfig: true,
      };

    case successT(FETCH_CONFIGURATION):
      return {
        ...state,
        currencyPairs: payload.currencyPairs ? payload.currencyPairs : {},
        isLoadingConfig: false,
      };

    case errorT(FETCH_CONFIGURATION):
      return {
        ...state,
        isLoadingConfig: false,
      };

    case startT(FETCH_RATES):
      return {
        ...state,
        isLoadingRates: true,
      };

    case successT(FETCH_RATES):
      return {
        ...state,
        current: payload.rates ? payload.rates : state.current,
        isLoadingRates: false,
        lastUpdate: new Date().toISOString(),
        previous: payload.rates ? state.current : state.previous,
      };

    case errorT(FETCH_RATES):
      return {
        ...state,
        isLoadingRates: false,
      };

    case SET_RATES:
      return {
        ...state,
        selected: payload,
      };

    default:
      return state;
  }
}
