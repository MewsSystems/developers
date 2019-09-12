import { combineReducers } from 'redux';

import currencyPairs from './currencyPairs.reducer';

export const createReducer = () =>
    combineReducers({
        currencyPairs
    });

