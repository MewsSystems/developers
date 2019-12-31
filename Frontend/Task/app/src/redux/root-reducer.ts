import { combineReducers } from 'redux';

import configurationReducer from './configuration/configuration.reducer';
import ratesReducer from './rates/rates.reducer'
import filterReducer from './filter/filter.reducer'

const rootReducer = combineReducers({
  configuration: configurationReducer,
  rates: ratesReducer,
  filter: filterReducer
});

export default rootReducer;