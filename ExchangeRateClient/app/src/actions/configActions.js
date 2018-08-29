import { api } from '../services';
import {
  FETCH_CONFIG_REQUEST,
  FETCH_CONFIG_SUCCESS,
  FETCH_CONFIG_ERROR,
  FETCH_RATES_REQUEST,
  FETCH_RATES_SUCCESS,
  FETCH_RATES_ERROR,
  PAIR_TOGGLE,
} from '../constants/actionTypes';

export function fetchConfig () {
  return async dispatch => {

    function onSuccess (config) {
      dispatch(fetchConfigSuccess(config));

    }
    function onError (error) {
      dispatch(fetchConfigError())
    }

    var localConfig = localStorage.getItem('config');
    if(localConfig !== null){
      return onSuccess(JSON.parse(localConfig))
    }

    dispatch(fetchConfigRequest())

    try {
      var config = await api.fetchConfig();
      localStorage.setItem('config', JSON.stringify(config));
      return onSuccess(config)
    } catch (error) {
      return onError('Error fetching config: ' + error.message)
    }
  }
  function fetchConfigRequest () { return { type: FETCH_CONFIG_REQUEST } }
  function fetchConfigSuccess (config) { return { type: FETCH_CONFIG_SUCCESS, payload: config} }
  function fetchConfigError () { return { type: FETCH_CONFIG_ERROR } }
}


export function fetchRates () {
  return async (dispatch, getState) => {
    function onSuccess (rates) {
      dispatch(fetchRatesSuccess(rates))
    }
    function onError (error) {
      dispatch(fetchRatesError())
    }

    dispatch(fetchRatesRequest())
    const pairs = getState().config.config
    try {
      var rates = await api.fetchRates(pairs)
      return onSuccess(rates)
    } catch (error) {
      return onError('Error fetching rates: ' + error.message)
    }
  }
  function fetchRatesRequest () { return { type: FETCH_RATES_REQUEST } }
  function fetchRatesSuccess (rates) { return { type: FETCH_RATES_SUCCESS, payload: rates } }
  function fetchRatesError () { return { type: FETCH_RATES_ERROR } }
}

export function pairToggle(id){

  var localConfig = localStorage.getItem('config');
  if(localConfig !== null){
    var config = JSON.parse(localConfig);
    config[id].selected = !config[id].selected;
    localStorage.setItem('config', JSON.stringify(config));
  }
  return{
    type: PAIR_TOGGLE,
    payload: id,
  }
}

export const actions = {
  fetchConfig,
  fetchRates,
  pairToggle,
}