import { FETCH_CONFIGURATION, FETCH_RATES, SET_RATES } from './actions';

export const startT = type => `${type}_PENDING`;
export const successT = type => `${type}_FULFILLED`;
export const errorT = type => `${type}_REJECTED`;

const initialState = {
  currencyPairs: {},
  previous: {},
  current: {},
  selected: [],
  configStatus: {
    isLoading: false,
    isRejected: false,
    error: null,
  },
  ratesStatus: {
    isLoading: false,
    isRejected: false,
    error: null,
  },
  lastUpdate: null,
};

export default function(state = initialState, action) {
  const { payload, type } = action;

  switch (type) {
    case startT(FETCH_CONFIGURATION):
      return {
        ...state,
        configStatus: {
          error: null,
          isLoading: true,
          isRejected: false,
        },
      };

    case successT(FETCH_CONFIGURATION):
      return {
        ...state,
        configStatus: {
          error: null,
          isLoading: false,
          isRejected: false,
        },
        currencyPairs:
          payload && payload.currencyPairs ? payload.currencyPairs : {},
      };

    case errorT(FETCH_CONFIGURATION):
      return {
        ...state,
        configStatus: {
          error: payload.data.message,
          isLoading: false,
          isRejected: true,
        },
      };

    case startT(FETCH_RATES):
      return {
        ...state,
        ratesStatus: {
          error: null,
          isLoading: true,
          isRejected: false,
        },
      };

    case successT(FETCH_RATES):
      return {
        ...state,
        ratesStatus: {
          error: null,
          isLoading: false,
          isRejected: false,
        },
        previous: payload.rates ? state.current : state.previous,
        current: payload.rates ? payload.rates : state.current,
        lastUpdate: new Date().toISOString(),
      };

    case errorT(FETCH_RATES):
      return {
        ...state,
        ratesStatus: {
          error: payload.data.message,
          isLoading: false,
          isRejected: true,
        },
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
