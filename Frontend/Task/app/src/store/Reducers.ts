import { StoreShape, CurrencyPairWithId } from "../models/StoreShape";
import {
  Action,
  ActionTypes,
  LoadConfigAction,
  UpdateRatesAction,
  TogglePairVisibilityAction,
  SetConfigLoadedAction,
  SetFirstRatesLoadedAction,
  SaveRatesAction
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
    case ActionTypes.SAVE_RATES: {
      /**Recreate the currency pair with id populated with rates */
      let saveRateAction = action as SaveRatesAction;
      let ids = Object.keys(saveRateAction.payload);
      let returnObject: CurrencyPairWithId = {};
      ids.forEach(id => {
        returnObject[id] = {
          ...state[id],
          rate: saveRateAction.payload[id],
          trend: Trend.STABLE
        };
      });
      return returnObject;
    }
    case ActionTypes.LOAD_CONFIG: {
      let loadAction = action as LoadConfigAction;
      let {
        payload
      } = loadAction; /**Z payloadu chceme vytvorit náš želaný state */

      /**Vytvoríme 1 currency pair */
      /**chceme dostat currency pairs do curr. pairs with ids */
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
    case ActionTypes.UPDATE_RATES: {
      var updateRatesAction = action as UpdateRatesAction;
      let { payload } = updateRatesAction;
      let currencyPairWithId: CurrencyPairWithId = {};
      for (let key in payload) {
        let newRate = payload[key];
        let oldRate = state[key].rate;
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
    case ActionTypes.TOGGLE_PAIR_VISIBILITY: {
      let togglePairVisibilityAction = action as TogglePairVisibilityAction;
      let id = togglePairVisibilityAction.id;

      let currencyPairWithId: CurrencyPairWithId = {};
      for (let key in state) {
        if (key == id) {
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

export function appReducer(state = initialState, action: Action): StoreShape {
  return {
    currencyPairs: currencyPairReducer(state.currencyPairs, action),
    configLoaded: configLoadedReducer(state.configLoaded, action),
    firstRatesLoaded: firstRatesLoadedReducer(state.firstRatesLoaded, action)
  };
}
