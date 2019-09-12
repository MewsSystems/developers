import { combineReducers } from 'redux';

import configuration from './currencyPairs.reducer';

export const createReducer = () =>
    combineReducers({
        configuration
    });

