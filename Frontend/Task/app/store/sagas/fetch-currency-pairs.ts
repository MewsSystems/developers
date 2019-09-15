import "regenerator-runtime/runtime";

import { call, put, takeLatest } from 'redux-saga/effects';

import { fetchCurrencyPairsApi, FetchCurrencyPairsApiResponse } from '@services/fetchCurrencyPairsApi';

import { Actions as CurrencyActions, types } from '@store/reducers/currency-pairs.reducer';
import { Actions as RatesActions } from '@store/reducers/rates.reducer';

import { CurrencyState } from '../types';

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
                } as CurrencyState;

                yield put(CurrencyActions.updateState(payload));
                yield put(RatesActions.fetchRatesPolling(currencyPairsIds));

            } else {
                yield put(CurrencyActions.updateState({ loading: false } as CurrencyState));

            }
        } catch (err) {
            yield put(CurrencyActions.updateState({ loading: false } as CurrencyState));
        }
    });
}
