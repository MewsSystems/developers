import {WATCH_TOGGLED} from '../const/action-names';
import {onConfigLoaded, onRatesLoaded, onWatchStart} from '../actions';
import {getCurrencyPairs, getIsWatching} from '../selectors';
import {getRates, getConfig} from '../utils/api';


const watchRates = (pairs, onRateLoad, interval = 10000) => {
  return setInterval(() => {
    getRates(Object.keys(pairs))
      .then(data => onRateLoad(data.rates))
      .catch(e => console.log('Rate loading was failed'));
  }, interval);
}

const loadConfig = () => new Promise((resolve, reject) => {
  getConfig()
    .then(resolve)
    .catch(getConfig)
    .then(resolve)
    .catch(getConfig)
    .then(resolve)
    .catch(reject);
});

let watcher;
let watchAutorun = true;
let isConfigLoaded = false;

const rateWatcher = config => store => next => action => {
  next(action);

  const state = store.getState();
  const isWatching = getIsWatching(state);

  if (!isConfigLoaded) {
    isConfigLoaded = true;
    loadConfig()
      .then(config => store.dispatch(onConfigLoaded(config)))
      .catch(() => console.log('Config isn\'t available :('));
  }

  if (watchAutorun) {
    watchAutorun = false;
    store.dispatch(onWatchStart());
  }

  if (isWatching && !watcher) {
    watcher = setInterval(() => {
      const pairIds = Object.keys(getCurrencyPairs(store.getState()));

      if (pairIds.length === 0) {
        return;
      }

      getRates(pairIds)
        .then(data => store.dispatch(onRatesLoaded(data.rates)))
        .catch(e => {});
    }, config.interval);
  }

  if (!isWatching && watcher) {
    clearInterval(watcher);
    watcher = null;
  }
};

export {rateWatcher};
