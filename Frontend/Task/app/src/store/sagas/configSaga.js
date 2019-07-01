import { put } from 'redux-saga/effects';
import * as R from 'ramda';

import * as actions from '../actions';

export function* fetchConfig(action) {
  try {
    const response = yield fetch('http://localhost:3000/configuration').then(
      res => res.json()
    );
    yield put(actions.fetchConfigSuccess(response.currencyPairs));
    yield put(actions.updateRates());
  } catch (error) {
    console.log(error);
  }
}
