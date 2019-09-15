import "regenerator-runtime/runtime";

import { call, takeLatest, put, select } from 'redux-saga/effects';
import { delay } from 'redux-saga';

import { fetchRatesApi, FetchRatesApiResponse } from '@services/fetchRatesApi';

import { Actions, types } from '@store/reducers/currencyPairs.reducer';
import { TIME_TO_UPDATE } from "@constants/config";
import { ApplicationState } from "@store/types";
import { normalizeRates } from "@utils/normalizeRates";

export default function* watchFetchRatesPolling() {
    yield takeLatest(types.FETCH_RATES_POLLING, function* (action: any) {
        while (true) {
            const { rates } = yield select();
            const responseData: FetchRatesApiResponse = yield call(fetchRatesApi, action.payload);

            if (responseData.success) {
                const payload = {
                    rates: normalizeRates(rates, responseData.rates)
                } as ApplicationState;

                yield put(Actions.updateState(payload));

            } else {


            }

            yield delay(TIME_TO_UPDATE);
          }
    });
}
