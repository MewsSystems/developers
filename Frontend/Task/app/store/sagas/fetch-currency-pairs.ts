import "regenerator-runtime/runtime";

import { call, put, takeLatest } from 'redux-saga/effects';

import { fetchCurrencyPairsApi, FetchCurrencyPairsApiResponse } from '@services/fetchCurrencyPairsApi';

import { Actions, types } from '@store/reducers/currencyPairs.reducer';

import { ApplicationState } from '../types';

export default function* watchFetchCurrencyPairs() {
    yield takeLatest(types.FETCH_CURRENCY_PAIRS, function* (action: any) {
        try {
            const responseData: FetchCurrencyPairsApiResponse = yield call(fetchCurrencyPairsApi);

            if (responseData.success) {
                const currencyPairs = responseData.currencyPairs;
                let currencyPairsIds: string[] = Object.keys(currencyPairs);

                const payload = {
                    currencyPairs,
                    currencyPairsIds,
                    loading: false
                } as ApplicationState;

                yield put(Actions.updateState(payload));
                yield put(Actions.fetchRatesPolling(currencyPairsIds));

            } else {
                yield put(Actions.updateState({ loading: false } as ApplicationState));

            }
        } catch (err) {
            yield put(Actions.updateState({ loading: false } as ApplicationState));
        }
    });
}
