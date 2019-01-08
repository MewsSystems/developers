import {combineReducers} from 'redux';

import pairsReducer from './pairsReducer';

const combinedReducers = combineReducers ({
  pairsReducer,
});

export default combinedReducers;
