import { take, all, takeLatest, cancel, fork } from 'redux-saga/effects';

import * as actionTypes from '../actions/actionTypes';
import { fetchConfig } from './configSaga';
import { syncRates } from './ratesSaga';

export function* fetchConfigSaga() {
  yield all([takeLatest(actionTypes.FETCH_CONFIG_INIT, fetchConfig)]);
  yield all([takeLatest(actionTypes.UPDATE_RATES, syncRates)]);
  while (yield take(actionTypes.UPDATE_RATES)) {
    // starts the task in the background
    const syncRatesTask = yield fork(syncRates);

    // wait for the user stop action
    yield take(actionTypes.FETCH_RATES_RETRY);
    // user clicked stop. cancel the background task
    // this will cause the forked bgSync task to jump into its finally block
    yield cancel(syncRatesTask);
  }
}
