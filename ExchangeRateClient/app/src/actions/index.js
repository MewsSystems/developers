import {CONFIG_LOADED, RATES_LOADED} from '../const/action-names';

export const onConfigLoaded = (config) => ({
  type: CONFIG_LOADED,
  payload: config,
});

export const onRatesLoaded = (rates) => ({
  type: RATES_LOADED,
  payload: rates,
});
