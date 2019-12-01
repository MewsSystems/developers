import {
  CurrencyPairConfigDTO,
  RatesDTO,
  LocalStorageDTO
} from "../models/DTOs";

export enum ActionTypes {
  SET_CONFIG_LOADED = "SET_CONFIG_LOADED" /**Sets the config loaded property */,
  SET_FIRST_RATES_LOADED = "SET_FIRST_RATES_LOADED" /**Sets the rates  */,
  LOAD_CONFIG = "LOAD_CONFIG" /** Loads the config from the server and creates */,
  SAVE_RATES = "SAVE_RATES" /**save rates for the first time */,
  UPDATE_RATES = "UPDATE_RATES" /**Updates the rates */,
  TOGGLE_PAIR_VISIBILITY = "TOGGLE_PAIR_VISIBILITY" /**Toggles the hidden property */,
  LOAD_CONFIG_LOCAL_STORAGE = "LOAD_CONFIG_LOCAL_STORAGE"
}

export interface SetConfigLoadedAction {
  type: ActionTypes.SET_CONFIG_LOADED;
  value: Boolean;
}

export interface SetFirstRatesLoadedAction {
  type: ActionTypes.SET_FIRST_RATES_LOADED;
  value: Boolean;
}
export interface LoadConfigAction {
  type: ActionTypes.LOAD_CONFIG;
  payload: CurrencyPairConfigDTO;
}
export interface SaveRatesAction {
  type: ActionTypes.SAVE_RATES;
  payload: RatesDTO;
}
export interface UpdateRatesAction {
  type: ActionTypes.UPDATE_RATES;
  payload: RatesDTO;
}
export interface LoadConfigLocalStorageAction {
  type: ActionTypes.LOAD_CONFIG_LOCAL_STORAGE;
  payload: LocalStorageDTO;
}

export interface TogglePairVisibilityAction {
  type: ActionTypes.TOGGLE_PAIR_VISIBILITY;
  id: String;
}

export type Action =
  | SetConfigLoadedAction
  | SetFirstRatesLoadedAction
  | LoadConfigAction
  | SaveRatesAction
  | UpdateRatesAction
  | TogglePairVisibilityAction
  | LoadConfigLocalStorageAction;
export function setConfigLoaded(value: Boolean): SetConfigLoadedAction {
  return {
    type: ActionTypes.SET_CONFIG_LOADED,
    value: value
  };
}

export function setFirstRatesLoaded(value: Boolean): SetFirstRatesLoadedAction {
  return {
    type: ActionTypes.SET_FIRST_RATES_LOADED,
    value: value
  };
}

export function loadConfigAction(
  payload: CurrencyPairConfigDTO
): LoadConfigAction {
  return {
    type: ActionTypes.LOAD_CONFIG,
    payload: payload
  };
}

export function saveRatesAction(payload: RatesDTO): SaveRatesAction {
  return {
    type: ActionTypes.SAVE_RATES,
    payload: payload
  };
}

export function updateRatesAction(payload: RatesDTO): UpdateRatesAction {
  return {
    type: ActionTypes.UPDATE_RATES,
    payload: payload
  };
}
export function togglePairVisibilityAction(
  id: String
): TogglePairVisibilityAction {
  return {
    type: ActionTypes.TOGGLE_PAIR_VISIBILITY,
    id: id
  };
}
export function loadConfigLocalStorageAction(
  payload: LocalStorageDTO
): LoadConfigLocalStorageAction {
  return {
    type: ActionTypes.LOAD_CONFIG_LOCAL_STORAGE,
    payload
  };
}
