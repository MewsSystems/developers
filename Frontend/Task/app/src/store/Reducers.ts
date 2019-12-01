import { StoreShape, CurrencyPairWithId } from "../models/StoreShape";
import {
  Action,
  ActionTypes,
  LoadConfigAction,
  UpdateRatesAction,
  TogglePairVisibilityAction,
  SetConfigLoadedAction,
  SetFirstRatesLoadedAction,
  SaveRatesAction,
  LoadConfigLocalStorageAction
} from "./Actions";
import CurrencyPair from "../models/Pair";
import Trend from "../models/Trend";

const initialState: StoreShape = {
  currencyPairs: {},
  configLoaded: false,
  firstRatesLoaded: false
};

function currencyPairReducer(
  state: CurrencyPairWithId,
  action: Action
): CurrencyPairWithId {
  switch (action.type) {
    /** Used to save rates on first /rates call */
    case ActionTypes.SAVE_RATES: {
      /**Recreate the currency pair with id populated with rates */
      let { payload } = action as SaveRatesAction;
      let returnObject: CurrencyPairWithId = {};
      for (let id in payload) {
        returnObject[id] = {
          ...state[id],
          rate: payload[id],
          trend: Trend.STABLE
        };
      }

      return returnObject;
    }
    /** */
    case ActionTypes.LOAD_CONFIG: {
      let loadAction = action as LoadConfigAction;
      let {
        payload
      } = loadAction; /**Z payloadu chceme vytvorit náš želaný state */

      let currencyPairWithId: CurrencyPairWithId = {};
      for (let key in payload) {
        let currencyPair: CurrencyPair = {
          currencies: payload[key],
          shown: true,
          rate: 0.0,
          trend: Trend.STABLE
        };
        currencyPairWithId[key] = currencyPair;
      }
      return currencyPairWithId;
    }
    /**Used to set the currencyPairs with object from localstorage */
    case ActionTypes.LOAD_CONFIG_LOCAL_STORAGE: {
      let loadAction = action as LoadConfigLocalStorageAction;

      let {
        payload
      } = loadAction; /**Z payloadu chceme vytvorit náš želaný state */

      let currencyPairWithId: CurrencyPairWithId = {};
      for (let key in payload) {
        let currencyPair: CurrencyPair = {
          currencies: payload[key].currencies,
          shown: payload[key].shown,
          rate: 0.0,
          trend: Trend.STABLE
        };
        currencyPairWithId[key] = currencyPair;
      }

      return currencyPairWithId;
    }
    /**Called when rates are already populated in state, so the second etc call of /rates, sets the trend in the copied object */
    case ActionTypes.UPDATE_RATES: {
      var updateRatesAction = action as UpdateRatesAction;
      let { payload } = updateRatesAction;
      let currencyPairWithId: CurrencyPairWithId = {};
      for (let key in payload) {
        let newRate: Number = payload[key];
        let oldRate: Number = state[key].rate;
        if (newRate > oldRate) {
          currencyPairWithId[key] = {
            ...state[key],
            trend: Trend.RAISING,
            rate: newRate
          };
        } else if (newRate < oldRate) {
          currencyPairWithId[key] = {
            ...state[key],
            trend: Trend.FALLING,
            rate: newRate
          };
        } else {
          currencyPairWithId[key] = { ...state[key], trend: Trend.STABLE };
        }
      }
      return currencyPairWithId;
    }
    /**Returns state object with one pair with [id] visibility changed */
    case ActionTypes.TOGGLE_PAIR_VISIBILITY: {
      let togglePairVisibilityAction = action as TogglePairVisibilityAction;
      let id = togglePairVisibilityAction.id;

      let currencyPairWithId: CurrencyPairWithId = {};
      for (let key in state) {
        if (key === id) {
          currencyPairWithId[key] = { ...state[key] };
          currencyPairWithId[key].shown = !currencyPairWithId[key].shown;
        } else {
          currencyPairWithId[key] = { ...state[key] };
        }
      }

      return currencyPairWithId;
    }
    default: {
      return state;
    }
  }
}
function configLoadedReducer(state: Boolean, action: Action): Boolean {
  switch (action.type) {
    case ActionTypes.SET_CONFIG_LOADED: {
      let setConfigLoadedAction: SetConfigLoadedAction = action as SetConfigLoadedAction;
      return setConfigLoadedAction.value;
    }
    default:
      return state;
  }
}
function firstRatesLoadedReducer(state: Boolean, action: Action): Boolean {
  switch (action.type) {
    case ActionTypes.SET_FIRST_RATES_LOADED: {
      let firstRatesLoadedAction = action as SetFirstRatesLoadedAction;
      return firstRatesLoadedAction.value;
    }
    default:
      return state;
  }
}

/**
 *
 * @param state StoreShape object
 * @param action Action object with payload
 * Returns new StoreShape
 */
export function appReducer(state = initialState, action: Action): StoreShape {
  return {
    currencyPairs: currencyPairReducer(state.currencyPairs, action),
    configLoaded: configLoadedReducer(state.configLoaded, action),
    firstRatesLoaded: firstRatesLoadedReducer(state.firstRatesLoaded, action)
  };
}
