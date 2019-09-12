import { call, put, takeLatest } from 'redux-saga/effects';

import { fetchConfigurationApi, FetchConfigurationApiResponse } from '../../services/fetchConfigurationApi';

import { Actions, types } from '../reducers/currencyPairs.reducer';

import { ConfigurationState } from '../types';
import _keyBy from 'lodash-es/keyBy';

export default function* watchFetchConfiguration() {
    yield takeLatest(types.FETCH_CONFIGURATION, function* (action: any) {
        try {
            const responseData: FetchConfigurationApiResponse = yield call(fetchConfigurationApi);

            if (responseData.success) {
                const payload = {
                    currencyPairs: responseData.currencyPairs,
                    loading: false
                } as ConfigurationState;

                yield put(Actions.updateState(payload));
            } else {
                yield put(Actions.updateState({ loading: false } as ConfigurationState));

            }
        } catch (err) {
            yield put(Actions.updateState({ loading: false } as ConfigurationState));
        }
    });
}
