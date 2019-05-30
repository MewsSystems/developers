import { combineReducers } from 'redux';

import currencies from './currencies';

const appReducer = combineReducers({
  currencies,
});

export default appReducer;
