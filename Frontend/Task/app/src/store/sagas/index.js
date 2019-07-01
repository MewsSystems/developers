import { takeEvery, all, takeLatest } from 'redux-saga/effects';

import * as actionTypes from '../actions/actionTypes';
import { fetchConfig } from './configSaga';
import { fetchRates } from './ratesSaga';

export function* fetchConfigSaga() {
  yield all([takeLatest(actionTypes.FETCH_CONFIG_INIT, fetchConfig)]);
  yield all([takeLatest(actionTypes.FETCH_RATES_INIT, fetchRates)]);
}
