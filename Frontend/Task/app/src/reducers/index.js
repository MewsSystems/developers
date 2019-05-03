import { combineReducers } from 'redux';
import currenciesReducer from './currenciesReducer';
import ratesReducer from './ratesReducer';

export default combineReducers({
  currencies: currenciesReducer,
  rates: ratesReducer,
});
