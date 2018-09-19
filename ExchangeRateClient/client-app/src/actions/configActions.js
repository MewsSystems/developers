// @flow strict
import axios from "axios";
import type { StateTypes } from "../store/types";
import logger from "../services/logger";

import { fetchConfiguration, fetchRates } from "../services/requests";

export type Action =
  | { type: "CONFIG_FETCH_SUCCESS", payload: any }
  | { type: "CONFIGS_FETCH_START" }
  | { type: "CONFIGS_FETCH_FAIL", payload: { error: Object } }
  | { type: "RATES_FETCH_START", payload: any }
  | { type: "RATES_FETCH_SUCCESS", payload: any }
  | { type: "RATES_FETCH_FAIL", payload: { error: Object } }
  | { type: "SELECT_RATES_IDS", payload: { ids: string[] } };

type GetState = () => StateTypes;
type PromiseAction = Promise<Action>;
export type ThunkAction = (dispatch: Dispatch, getState: GetState) => any;
type Dispatch = (action: Action | ThunkAction | PromiseAction | Array<Action>) => any;

export const CONFIG_FETCH_SUCCESS = "CONFIG_FETCH_SUCCESS";
export const CONFIGS_FETCH_START = "CONFIGS_FETCH_START";
export const CONFIGS_FETCH_FAIL = "CONFIGS_FETCH_FAIL";
export const RATES_FETCH_START = "RATES_FETCH_START";
export const RATES_FETCH_SUCCESS = "RATES_FETCH_SUCCESS";
export const RATES_FETCH_FAIL = "RATES_FETCH_FAIL";
export const SELECT_RATES_IDS = "SELECT_RATES_IDS";

const fetchConfigSuccess = data => ({
  type: CONFIG_FETCH_SUCCESS,
  payload: data,
});

// TODO check name
const fetchConfigurationStart = () => ({
  type: CONFIGS_FETCH_START,
});

const fetchConfigFail = (error: Error) => ({
  type: CONFIGS_FETCH_FAIL,
  payload: { error },
});

const fetchRatesStart = ids => ({
  type: RATES_FETCH_START,
  payload: ids,
});

const fetchRatesSuccess = data => ({
  type: RATES_FETCH_SUCCESS,
  payload: data,
});

const fetchRatesFail = (error: Error) => ({
  type: RATES_FETCH_FAIL,
  payload: { error },
});

export const selectIds = (ids: string[]) => ({
  type: SELECT_RATES_IDS,
  payload: {
    ids,
  },
});

export const fetchConfigAction = (): ThunkAction =>
  function(dispatch) {
    dispatch(fetchConfigurationStart());
    return fetchConfiguration()
      .then(data => dispatch(fetchConfigSuccess(data)))
      .catch(err => {
        logger(err);
        dispatch(fetchConfigFail(err));
      });
  };

export const fetchRatesAction = (ids: string[]): ThunkAction =>
  function(dispatch) {
    dispatch(fetchRatesStart(ids));
    return fetchRates(ids)
      .then(data => dispatch(fetchRatesSuccess(data)))
      .catch(err => {
        logger(err);
        dispatch(fetchRatesFail(err));
      });
  };
