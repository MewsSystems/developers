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

const normalizeConfig = ({currencyPairs}) => {
  let normalized = {};
  Object.keys(currencyPairs).forEach(o => {
    normalized[o] = {
      selected: false,
      baseCode: currencyPairs[o][0].code,
      baseName: currencyPairs[o][0].name,
      secondaryCode: currencyPairs[o][1].code,
      secondaryName: currencyPairs[o][1].name,
      oldRate: null,
      newRate: null,
    }
  });
  return normalized;
}

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
      const normalized = normalizeConfig(config);
      localStorage.setItem('config', JSON.stringify(normalized));
      return onSuccess(normalized)
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
    var ids = []
    Object.keys(pairs).forEach(o => {
      if(pairs[o].selected) ids.push(o);
    })
    try {
      var rates = await api.fetchRates(ids)
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