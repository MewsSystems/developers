import { combineReducers, } from 'redux';

import rateListReducer from '../modules/RateList/reducers/rateListReducer';
import ratesConfigurationReducer from '../modules/Main/dataReducers/ratesConfigurationReducer';
import ratesReducer from '../modules/Main/dataReducers/ratesReducer';


/**
 * Reducer Tree
 */
export default combineReducers({
  data: combineReducers({
    ratesConfiguration: ratesConfigurationReducer,
    rates: ratesReducer,
  }),
  rateListPage: combineReducers({
    rateList: rateListReducer,
  }),
});
