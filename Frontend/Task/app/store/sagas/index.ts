import { all, fork } from 'redux-saga/effects';

import fetchConfigurationSagas from './fetch-configuration';
import fetchRatesSaga from './fetch-rates';
import fetchRatesPollingSaga from './fetch-rates-polling';

export function* rootSaga() {
    yield all([
        fork(fetchConfigurationSagas),
        fork(fetchRatesSaga),
        fork(fetchRatesPollingSaga)
    ])
}