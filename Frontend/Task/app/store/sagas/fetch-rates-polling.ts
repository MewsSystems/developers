import "regenerator-runtime/runtime";
import React from 'react';
import { call, put, takeLatest } from 'redux-saga/effects';
import { delay } from 'redux-saga';

import { fetchRatesApi, FetchRatesApiResponse } from '../../services/fetchRatesApi';

import { Actions, types } from '../reducers/currencyPairs.reducer';

import { ConfigurationState, CurrencyPair } from '../types';
import fetchRates from "./fetch-rates";
import watchFetchRates from "./fetch-rates";

export default function* watchFetchRatesPolling() {
    yield takeLatest(types.FETCH_RATES_POLLING, function* (action: any) {
        while (true) {
            const responseData: FetchRatesApiResponse = yield call(fetchRatesApi, action.payload);

            if (responseData.success) {



            } else {


            }

            yield delay(5000);
          }
    });
}
