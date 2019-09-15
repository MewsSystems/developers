import { combineReducers } from 'redux';

import currencyState from './currency-pairs.reducer';
import ratesState from './rates.reducer';
import alert from './alert.reducer';

export const createReducer = () =>
    combineReducers({
        currencyState,
        ratesState,
        alert,
    });