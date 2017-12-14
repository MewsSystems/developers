import {combineReducers} from 'redux';

import filter from './filter/filterReducer';
import configuration from './configuration/configurationReducer';
import rates from './rates/ratesReducer';

export default combineReducers({
    filter,
    configuration,
    rates,
});
