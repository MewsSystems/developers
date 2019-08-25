import { combineReducers } from 'redux';
import { reducer as notificationsReducer } from 'reapop';
import currencyPairsReducer from 'Reducers/currencyPairsReducer';
import ratesReducer from 'Reducers/ratesReducer';
import filterReducer from 'Reducers/filterReducer';

export default combineReducers({
  pairs: currencyPairsReducer,
  rates: ratesReducer,
  filters: filterReducer,
  notifications: notificationsReducer()
});
