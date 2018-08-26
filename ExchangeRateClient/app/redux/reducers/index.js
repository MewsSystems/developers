// @flow
import { combineReducers } from 'redux';
import rates from './rates';
import pairs from './pairs';
import trends from './trends';

const rootReducer = combineReducers({
  rates,
  pairs,
  trends,
});

export default rootReducer;
