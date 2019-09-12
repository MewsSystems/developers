import { all, fork } from 'redux-saga/effects';

import fetchConfigurationSagas from './fetch-configuration';
import fetchRatesPollingSaga from './fetch-rates-polling';

export function* rootSaga() {
    yield all([
        fork(fetchConfigurationSagas),
        fork(fetchRatesPollingSaga)
    ])
}