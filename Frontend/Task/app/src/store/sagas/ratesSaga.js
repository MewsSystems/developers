import { delay } from 'redux-saga/effects';
import { put, call } from 'redux-saga/effects';
import * as R from 'ramda';
import qs from 'qs';

import * as actions from '../actions';

export function* fetchRates(action) {
  try {
    const response = yield fetch(
      `http://localhost:3000/rates?${qs.stringify({
        currencyPairIds: action.payload
      })}`
    ).then(res => {
      console.log(res.status);
      return res.json();
    });
    yield put(actions.fetchRatesSuccess(response.rates));
    yield updateRates(action.payload);
  } catch (error) {
    yield put(actions.fetchRatesFail(error));
  }
}
function* updateRates(payload) {
  console.log('I fire');
  while (true) {
    yield delay(10000);
    try {
      const response = yield fetch(
        `http://localhost:3000/rates?${qs.stringify({
          currencyPairIds: payload
        })}`
      ).then(res => {
        console.log(res.status);
        return res.json();
      });
      yield put(actions.updateRates(response.rates));
    } catch (error) {
      yield put(actions.fetchRatesFail(error));
    }
  }
}
