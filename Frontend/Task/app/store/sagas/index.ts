import { all, fork } from 'redux-saga/effects';

import fetchCurrencyPairsSagas from './fetch-currency-pairs';
import fetchRatesPollingSaga from './fetch-rates-polling';

export function* rootSaga() {
    yield all([
        fork(fetchCurrencyPairsSagas),
        fork(fetchRatesPollingSaga)
    ])
}