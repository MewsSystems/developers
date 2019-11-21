import {combineReducers} from 'redux';

import pairs from './pairs';

const combinedReducers = combineReducers ({
  pairs,
});

export default combinedReducers;
