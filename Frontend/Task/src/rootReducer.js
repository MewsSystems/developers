import { combineReducers } from 'redux';
import rates from './features/Rates/reducer';

const rootReducer = combineReducers({
  rates,
});

export default rootReducer;
