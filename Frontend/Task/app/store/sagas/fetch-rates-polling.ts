import "regenerator-runtime/runtime";

import { call, takeLatest } from 'redux-saga/effects';
import { delay } from 'redux-saga';

import { fetchRatesApi, FetchRatesApiResponse } from '../../services/fetchRatesApi';

import { Actions, types } from '../reducers/currencyPairs.reducer';
import { TIME_TO_UPDATE } from "../../constants";

export default function* watchFetchRatesPolling() {
    yield takeLatest(types.FETCH_RATES_POLLING, function* (action: any) {
        while (true) {
            const responseData: FetchRatesApiResponse = yield call(fetchRatesApi, action.payload);

            if (responseData.success) {



            } else {


            }

            yield delay(TIME_TO_UPDATE);
          }
    });
}
