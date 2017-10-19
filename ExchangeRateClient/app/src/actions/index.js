import {
  CONFIG_LOADED,
  RATES_LOADED,
  FILTER_CHANGED,
  FILTER_CLEARED,
  WATCH_TOGGLED,
  WATCH_STARTED
} from '../const/action-names';


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

export const onFilterClear = () => ({
  type: FILTER_CLEARED
});

export const onWatchToggle = () => ({
  type: WATCH_TOGGLED
});

export const onWatchStart = () => ({
  type: WATCH_STARTED
});
