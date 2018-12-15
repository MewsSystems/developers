import storage from "redux-persist/lib/storage";
import { persistReducer } from "redux-persist";
import { getConfigFromApi, getRatesFromApi } from "../../api/currencyApi";

const FETCH_CONFIG = "currency/FETCH_CONFIG";
const FETCH_CONFIG_SUCCESS = "currency/FETCH_CONFIG_SUCCESS";
const FETCH_CONFIG_ERROR = "currency/FETCH_CONFIG_ERROR";
const FETCH_RATES = "currency/FETCH_RATES";
const FETCH_RATES_SUCCESS = "currency/FETCH_RATES_SUCCESS";
const FETCH_RATES_ERROR = "currency/FETCH_RATES_ERROR";
const ACTIVE_CURRENCY_PAIRS_CHANGED = "currency/ACTIVE_CURRENCY_PAIRS_CHANGED";

const initialState = {
  fetchingConfig: false,
  currencyPairs: null,
  activeCurrencyPairs: [],
  previousRates: null,
  currentRates: null,
  updatedAt: null
};

const reducer = (state = initialState, action = {}) => {
  switch (action.type) {
    case FETCH_CONFIG:
      return { ...state, fetchingConfig: true };
    case FETCH_CONFIG_SUCCESS:
      return { ...state, fetchingConfig: false, currencyPairs: action.payload };
    case FETCH_CONFIG_ERROR:
      return { ...state, fetchingConfig: false };
    case FETCH_RATES_SUCCESS:
      return {
        ...state,
        previousRates: state.currentRates ? state.currentRates : null,
        currentRates: action.payload,
        updatedAt: Date.now()
      };
    case ACTIVE_CURRENCY_PAIRS_CHANGED:
      return { ...state, activeCurrencyPairs: action.payload };
    default:
      return state;
  }
};

// currencyPairs (config) is persisted on page refresh, when a new config is loaded on page load
// it overwrites the persisted config (e.g. when there are new currency pairs). Rest of the state remains untouched.
// This is handled by the default redux-persist state reconciler - autoMergeLevel1.
// Some state values shouldn't be persisted - they are blacklisted below.
const persistConfig = {
  key: "currency",
  storage: storage,
  blacklist: ["fetchingConfig", "previousRates", "currentRates", "updatedAt"]
};

export default persistReducer(persistConfig, reducer);

export const getConfig = () => dispatch => {
  dispatch({ type: FETCH_CONFIG });

  getConfigFromApi().then(
    data => {
      console.log("config data: ", data);
      dispatch({
        type: FETCH_CONFIG_SUCCESS,
        payload: data.data.currencyPairs
      });
    },
    error => {
      dispatch({ type: FETCH_CONFIG_ERROR });
    }
  );
};

export const getRates = () => (dispatch, getState) => {
  dispatch({ type: FETCH_RATES });
  const activeCurrencyPairs = getState().currency.activeCurrencyPairs;

  getRatesFromApi(activeCurrencyPairs).then(
    data => {
      console.log("rates data: ", data);
      dispatch({
        type: FETCH_RATES_SUCCESS,
        payload: data.data.rates
      });
    },
    error => {
      dispatch({ type: FETCH_RATES_ERROR });
    }
  );
};

export const updateActiveCurrencyPairs = activePairs => dispatch => {
  dispatch({ type: ACTIVE_CURRENCY_PAIRS_CHANGED, payload: activePairs });
};
