import { combineReducers, } from 'redux';

import rateListUIReducer from '../modules/RateList/reducers/rateListUIReducer';
import ratesConfigurationReducer from '../modules/RateList/reducers/ratesConfigurationReducer';
import ratesReducer from '../modules/RateList/reducers/ratesReducer';


/**
 * Reducer Tree
 */
export default combineReducers({
  data: combineReducers({
    ratesConfigurationReducer,
    ratesReducer,
  }),
  rateListPage: combineReducers({
    rateListUIReducer,
  }),
});
