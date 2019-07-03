import { put } from 'redux-saga/effects'
import promise from 'es6-promise'
import fetch from 'isomorphic-fetch'
import 'babel-polyfill'
promise.polyfill()

import * as actions from '../actions'

export function* fetchConfig() {
  try {
    yield getCofig()
  } catch (error) {
    console.log(error)
    yield put(actions.fetchConfigInit())
  }
}
function* getCofig() {
  try {
    const response = yield fetch('http://localhost:3000/configuration').then(
      res => {
        if (res.ok) {
          return res.json()
        } else {
          throw new Error('500')
        }
      }
    )
    yield localStorage.setItem('config', JSON.stringify(response.currencyPairs))
    yield put(actions.fetchConfigSuccess(response.currencyPairs))
    yield put(actions.syncRates())
  } catch (error) {
    console.log(error)
  }
}

export function* checkConfig() {
  const config = yield JSON.parse(localStorage.getItem('config'))
  if (!config) {
    yield put(actions.fetchConfig())
  } else {
    yield put(actions.fetchConfigSuccess(config))
    yield put(actions.syncRates())
  }
}
