import storage from "redux-persist/lib/storage";
import { persistReducer } from "redux-persist";
import { getConfiguration } from "../../api/currencyApi";

const FETCH_CONFIG = "currency/FETCH_CONFIG";
const FETCH_CONFIG_SUCCESS = "currency/FETCH_CONFIG_SUCCESS";
const FETCH_CONFIG_ERROR = "currency/FETCH_CONFIG_ERROR";
const ACTIVE_CURRENCY_PAIRS_CHANGED = "currency/ACTIVE_CURRENCY_PAIRS_CHANGED";

const initialState = {
  fetchingConfig: false,
  currencyPairs: null,
  activeCurrencyPairs: []
};

const reducer = (state = initialState, action = {}) => {
  switch (action.type) {
    case FETCH_CONFIG:
      return { ...state, fetchingConfig: true };
    case FETCH_CONFIG_SUCCESS:
      return { ...state, fetchingConfig: false, currencyPairs: action.payload };
    case FETCH_CONFIG_ERROR:
      return { ...state, fetchingConfig: false };
    case ACTIVE_CURRENCY_PAIRS_CHANGED:
      return { ...state, activeCurrencyPairs: action.payload };
    default:
      return state;
  }
};

const persistConfig = {
  key: "currency",
  storage: storage,
  blacklist: ["fetchingConfig"]
};

export default persistReducer(persistConfig, reducer);

export const getConfig = () => dispatch => {
  dispatch({ type: FETCH_CONFIG });

  getConfiguration().then(
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

export const updateActiveCurrencyPairs = activePairs => dispatch => {
  dispatch({ type: ACTIVE_CURRENCY_PAIRS_CHANGED, payload: activePairs });
};
