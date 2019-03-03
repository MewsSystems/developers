import {
  FETCH_CONFIGURATION,
  FETCH_RATES,
  SET_RATES,
  ADD_RATE,
  REMOVE_RATE,
} from './actions';

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
        currencyPairs: payload.currencyPairs,
      };

    case errorT(FETCH_CONFIGURATION):
      return {
        ...state,
        configStatus: {
          error: payload,
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
        previous: state.current,
        current: payload.rates,
      };

    case errorT(FETCH_RATES):
      return {
        ...state,
        ratesStatus: {
          error: payload,
          isLoading: false,
          isRejected: true,
        },
      };

    case SET_RATES:
    case ADD_RATE:
      const newSelected = [...state.rates.selected];
      return newSelected.push(payload);
      return {
        ...state,
        selectedRates: newSelected,
      };
    // case REMOVE_RATE:
    default:
      return state;
  }
}
