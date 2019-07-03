import { put, cancelled, select, delay } from 'redux-saga/effects'
import { keys } from 'ramda'
import qs from 'qs'
import promise from 'es6-promise'
import fetch from 'isomorphic-fetch'
import 'babel-polyfill'
promise.polyfill()

import * as actions from '../actions'
import { CurrencyListState, ConfigInterface } from '../../types'

const getConfig = (state: CurrencyListState) => state.config

export function* syncRates() {
  let config: ConfigInterface = yield select(getConfig)
  try {
    while (true) {
      yield fetchRates(keys(config))
      yield delay(10000)
    }
  } finally {
    if (yield cancelled()) {
      console.log('Sync cancelled, set to renew')
    }
  }
}
function* fetchRates(list: Array<string>) {
  try {
    const response = yield fetch(
      `http://localhost:3000/rates?${qs.stringify({
        currencyPairIds: list,
      })}`
    ).then(res => {
      if (res.ok) {
        return res.json()
      } else {
        throw new Error('500')
      }
    })
    yield put(actions.updateRatesSuccess(response.rates))
  } catch (error) {
    yield put(actions.fetchRatesFail(error))
    yield put(actions.syncRates())
  }
}
