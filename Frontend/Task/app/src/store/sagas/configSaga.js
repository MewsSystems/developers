import { delay } from 'redux-saga/effects';
import { put, call } from 'redux-saga/effects';
import * as R from 'ramda';

import * as actions from '../actions';

export function* fetchConfig(action) {
  try {
    const response = yield fetch('http://localhost:3000/configuration').then(
      res => res.json()
    );
    const formatted = yield response.currencyPairs;
    yield put(actions.fetchConfigSuccess(formatted));
    yield put(actions.fetchRatesInit(R.keys(formatted)));
  } catch (error) {
    console.log(error);
  }
}
