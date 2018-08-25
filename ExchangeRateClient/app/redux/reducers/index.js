// @flow
import { combineReducers } from 'redux';
import rates from './rates';
import pairs from './pairs';

const rootReducer = combineReducers({
  rates,
  pairs,
});

export default rootReducer;
