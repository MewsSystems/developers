import "regenerator-runtime/runtime";
import React from 'react';
import { call, put, takeLatest } from 'redux-saga/effects';

import { fetchRatesApi, FetchRatesApiResponse } from '../../services/fetchRatesApi';

import { Actions, types } from '../reducers/currencyPairs.reducer';

import { ConfigurationState, CurrencyPair } from '../types';

export default function* watchFetchRates() {
    yield takeLatest(types.FETCH_RATES, function* (action: any) {
        try {
            const responseData: FetchRatesApiResponse = yield call(fetchRatesApi, action.payload);

            if (responseData.success) {



            } else {


            }
        } catch (err) {

        }
    });
}
