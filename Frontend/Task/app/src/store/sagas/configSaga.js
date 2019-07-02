import { put } from 'redux-saga/effects';
import * as R from 'ramda';

import * as actions from '../actions';

export function* fetchConfig(action) {
  try {
    yield getCofig();
  } catch (error) {
    console.log(error);
    yield put(actions.fetchConfigInit());
  }
}
function* getCofig() {
  try {
    const response = yield fetch('http://localhost:3000/configuration').then(
      res => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error(500);
        }
      }
    );
    yield localStorage.setItem(
      'config',
      JSON.stringify(response.currencyPairs)
    );
    yield put(actions.fetchConfigSuccess(response.currencyPairs));
    yield put(actions.updateRates());
  } catch (error) {
    console.log(error);
  }
}

export function* checkConfig(action) {
  const config = yield JSON.parse(localStorage.getItem('config'));
  if (!config) {
    yield put(actions.fetchConfig());
  } else {
    yield put(actions.fetchConfigSuccess(config));
    yield put(actions.updateRates());
  }
}
