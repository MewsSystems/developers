import { combineReducers } from 'redux';

import configurationReducer from './configuration/configuration.reducer';
import ratesReducer from './rates/rates.reducer'

const rootReducer = combineReducers({
  configuration: configurationReducer,
  rates: ratesReducer
});

export default rootReducer;