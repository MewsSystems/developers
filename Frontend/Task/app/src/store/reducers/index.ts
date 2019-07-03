import { combineReducers } from 'redux';

import configReducer from './configReducer';
import ratesReducer from './ratesReducer';
import filterReducer from './filterReducer';

export default combineReducers({
  config: configReducer,
  rates: ratesReducer,
  filtered: filterReducer
});
