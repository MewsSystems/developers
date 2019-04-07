import { combineReducers } from 'redux';
import configuration from './configuration';
import rates from './rates';

const rootReducer = combineReducers({
  configuration,
  rates,
});

export default rootReducer;
