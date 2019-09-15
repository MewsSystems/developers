import "regenerator-runtime/runtime";

import { call, takeLatest, put, select } from 'redux-saga/effects';
import { delay } from 'redux-saga';

import { fetchRatesApi, FetchRatesApiResponse } from '@services/fetchRatesApi';
import { TIME_TO_UPDATE } from "@constants/config";
import { RatesState } from "@store/types";
import { normalizeRates } from "@utils/normalizeRates";

import { Actions as RatesActions, types } from '@store/reducers/rates.reducer';
import { Actions as AlertActions } from '../reducers/alert.reducer';

export default function* watchFetchRatesPolling() {
    yield takeLatest(types.FETCH_RATES_POLLING, function* (action: any) {
        while (true) {
            const { rates } = yield select();
            const responseData: FetchRatesApiResponse = yield call(fetchRatesApi, action.payload);

            if (responseData.success) {
                const payload = {
                    rates: normalizeRates(rates, responseData.rates)
                } as RatesState;

                yield put(RatesActions.updateState(payload));

            } else {
                yield put(AlertActions.showAlert({
                    show: true,
                    message: responseData.errorMessage
                }));
            }

            yield delay(TIME_TO_UPDATE);
        }
    });
}