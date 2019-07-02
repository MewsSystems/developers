import { take, all, takeLatest, cancel, fork } from 'redux-saga/effects';

import * as actionTypes from '../actions/actionTypes';
import { fetchConfig, checkConfig } from './configSaga';
import { syncRates } from './ratesSaga';

export function* fetchConfigSaga() {
  yield all([takeLatest(actionTypes.FETCH_CONFIG, fetchConfig)]);
  yield all([takeLatest(actionTypes.FETCH_CONFIG_INIT, checkConfig)]);
  yield all([takeLatest(actionTypes.UPDATE_RATES, syncRates)]);
  while (yield take(actionTypes.UPDATE_RATES)) {
    // starts the task in the background
    const syncRatesTask = yield fork(syncRates);

    yield take(actionTypes.UPDATE_RATES);

    yield cancel(syncRatesTask);
  }
}
