import { delay } from 'redux-saga/effects';
import { put, cancelled, select } from 'redux-saga/effects';
import * as R from 'ramda';
import qs from 'qs';

import * as actions from '../actions';

const getConfig = state => state.config;

export function* syncRates() {
  let config = yield select(getConfig);
  try {
    while (true) {
      yield fetchRates(R.keys(config));
      yield delay(10000);
    }
  } finally {
    if (yield cancelled()) {
      console.log('Show user that we actually care...');
    }
  }
}
function* fetchRates(list) {
  try {
    const response = yield fetch(
      `http://localhost:3000/rates?${qs.stringify({
        currencyPairIds: list
      })}`
    ).then(res => {
      if (res.ok) {
        return res.json();
      } else {
        throw new Error(500);
      }
    });
    yield put(actions.updateRatesSuccess(response.rates));
  } catch (error) {
    yield put(actions.fetchRatesFail(error));
    yield put(actions.updateRates());
  }
}
