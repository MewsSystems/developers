import { all, fork } from 'redux-saga/effects';

import configurationSagas from './configuration';

export function* rootSaga() {
    yield all([
        fork(configurationSagas)
    ])
}