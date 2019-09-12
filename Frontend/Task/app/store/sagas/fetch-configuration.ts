import "regenerator-runtime/runtime";

import { call, put, takeLatest } from 'redux-saga/effects';

import { fetchConfigurationApi, FetchConfigurationApiResponse } from '../../services/fetchConfigurationApi';

import { Actions, types } from '../reducers/currencyPairs.reducer';

import { ConfigurationState } from '../types';

export default function* watchFetchConfiguration() {
    yield takeLatest(types.FETCH_CONFIGURATION, function* (action: any) {
        try {
            const responseData: FetchConfigurationApiResponse = yield call(fetchConfigurationApi);

            if (responseData.success) {
                const currencyPairs = responseData.currencyPairs;
                let currencyPairsIdList: string[] = Object.keys(currencyPairs).map(key =>key);

                const payload = {
                    currencyPairs,
                    currencyPairsIdList,
                    loading: false
                } as ConfigurationState;

                yield put(Actions.updateState(payload));
                yield put(Actions.fetchRatesPolling(currencyPairsIdList));

            } else {
                yield put(Actions.updateState({ loading: false } as ConfigurationState));

            }
        } catch (err) {
            yield put(Actions.updateState({ loading: false } as ConfigurationState));
        }
    });
}
