import { take, all, takeLatest, cancel, fork } from 'redux-saga/effects'

import * as actionTypes from '../actions/actionTypes'
import { fetchConfig, checkConfig } from './configSaga'
import { syncRates } from './ratesSaga'

export function* fetchData() {
  yield all([takeLatest(actionTypes.FETCH_CONFIG, fetchConfig)])
  yield all([takeLatest(actionTypes.FETCH_CONFIG_INIT, checkConfig)])
  yield all([takeLatest(actionTypes.SYNC_RATES, syncRates)])
  while (yield take(actionTypes.FETCH_RATES_RETRY)) {
    // starts the task in the background
    const syncRatesTask = yield fork(syncRates)
    yield cancel(syncRatesTask)
    yield take(actionTypes.SYNC_RATES)
  }
}
