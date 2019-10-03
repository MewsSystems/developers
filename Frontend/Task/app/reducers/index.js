import {
  API_ERROR_CONFIGURATION,
  API_ERROR_RATES,
  API_RECEIVED_CONFIGURATION,
  API_RECEIVED_RATES,
  API_REQUESTED_CONFIGURATION,
  API_REQUESTED_RATES,
  UI_CURRENCY_FILTER,
  UI_CURRENCY_SELECT,
  UI_CURRENCY_DESELECT,
} from '../actions/types';

export const INITIAL_STATE = {
  currencyPairs: {}, // object with keys pairId: [{code, name}, {code, name}]
  // populated through initial /configuration API call (or from localStorage)
  errorFetchingConfiguration: '', // error message if API call fails
  errorFetchingRates: '', // error message if API call fails
  errorFetchingRatesCount: 0, // keep count of successive errors
  filter: '', // if not empty, filter currency pair codes/names against this string
  isFetchingConfiguration: false, // true if API request sent but not recieved
  isFetchingRates: false, // true if API request sent but not recieved
  rates: {}, // object with keys pairId: [value1, value2, ...] (oldest first)
  // new rates pushed to each array after each call to /rates API
  selectedCurrencyPairs: [], // array of pairIds
};

const reducer = (state = INITIAL_STATE, action) => {
  const { type, payload } = action;
  // console.log(`Action of type ${type}, payload is: ${JSON.stringify(payload, null, 2)}`);
  switch (type) {
    case API_ERROR_CONFIGURATION:
      // payload is error message
      return {
        ...state,
        errorFetchingConfiguration: payload || 'Unknown error',
        isFetchingConfiguration: false,
      };
    case API_ERROR_RATES:
    // payload is error message
      return {
        ...state,
        errorFetchingRates: payload || 'Unknown error',
        // increment count with each successive failure
        errorFetchingRatesCount: state.errorFetchingRatesCount + 1,
        isFetchingRates: false,
      };
    case API_RECEIVED_CONFIGURATION:
      // payload is response.currencyPairs
      return {
        ...state,
        currencyPairs: payload,
        errorFetchingConfiguration: '',
        isFetchingConfiguration: false,
      };
    case API_RECEIVED_RATES: {
      // payload is response.rates
      const newRates = { ...state.rates }; // new object
      Object.keys(payload).forEach((pairId) => {
        if (newRates[pairId]) {
          newRates[pairId] = [...newRates[pairId], payload[pairId]];
        } else {
          newRates[pairId] = [payload[pairId]];
        }
      });
      return {
        ...state,
        errorFetchingRates: '',
        errorFetchingRatesCount: 0, // reset after success
        isFetchingRates: false,
        rates: newRates,
      };
    }
    case API_REQUESTED_CONFIGURATION:
      // no payload
      return {
        ...state,
        isFetchingConfiguration: true,
      };
    case API_REQUESTED_RATES:
      // no payload
      return {
        ...state,
        isFetchingRates: true,
      };
    case UI_CURRENCY_FILTER:
      // payload is updated input string
      return {
        ...state,
        filter: payload,
      };
    case UI_CURRENCY_SELECT:
      // payload is pairId
      if (state.selectedCurrencyPairs.includes(payload)) {
        return state; // already selected, do nothing
      }
      if (!Object.keys((state.currencyPairs)).includes(payload)) {
        // pairId does not represent a valid currency pair, throw error as something
        // has gone very wrong
        throw new Error('Attempt to select an invalid currency pair');
      }
      return {
        ...state,
        selectedCurrencyPairs: [...state.selectedCurrencyPairs, payload],
      };
    case UI_CURRENCY_DESELECT:
      // payload is pairId
      if (state.selectedCurrencyPairs.includes(payload)) {
        // remove
        const newSelectedCurrencyPairs = state.selectedCurrencyPairs.filter((pairId) => {
          return (pairId !== payload);
        });
        return {
          ...state,
          selectedCurrencyPairs: newSelectedCurrencyPairs,
        };
      }
      if (!Object.keys((state.currencyPairs)).includes(payload)) {
        // pairId does not represent a valid currency pair, throw error as something
        // has gone very wrong
        throw new Error('Attempt to deselect an invalid currency pair');
      }
      return state; // valid but already not selected, do nothing
    default:
      return state;
  }
};

export default reducer;
