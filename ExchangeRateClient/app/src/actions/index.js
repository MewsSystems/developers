import {CONFIG_LOADED, RATES_LOADED, FILTER_CHANGED} from '../const/action-names';

export const onConfigLoaded = (config) => ({
  type: CONFIG_LOADED,
  payload: config,
});

export const onRatesLoaded = (rates) => ({
  type: RATES_LOADED,
  payload: rates,
});

export const onFilterChanged = (id) => ({
  type: FILTER_CHANGED,
  payload: id
});
