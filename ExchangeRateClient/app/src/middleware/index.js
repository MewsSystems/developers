import {REHYDRATE} from 'redux-persist/constants';
import {onConfigLoaded, onRatesLoaded} from '../actions';
import {getCurrencyPairs} from '../selectors';
import {getRates, getConfig} from '../utils/api';


const watchRates = (pairs, onRateLoad, interval = 10000) => {
  return setInterval(() => {
    getRates(Object.keys(pairs))
      .then(data => onRateLoad(data))
      .catch(e => console.log(e));
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

let isRateWatchActive = false;
let isConfigLoading = false;
const rateWatcher = config => store => next => action => {
  next(action);

  if (isRateWatchActive) {
    return;
  }

  if (!isConfigLoading) {
    isConfigLoading = true;
    loadConfig()
      .then(config => store.dispatch(onConfigLoaded(config)))
      .catch(() => console.log('Config isn\'t available :('));
  }

  const pairs = getCurrencyPairs(store.getState());
  if (pairs) {
    isRateWatchActive = true;
    watchRates(
      pairs,
      (rates) => store.dispatch(onRatesLoaded(rates)),
      config.interval
    );
  }
};

export {rateWatcher};
