import { combineReducers, } from 'redux';
import { persistReducer, } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import autoMergeLevel2 from 'redux-persist/lib/stateReconciler/autoMergeLevel2';

import rateListReducer from '../modules/RateList/reducers/rateListReducer';
import ratesConfigurationReducer from '../modules/Main/dataReducers/ratesConfigurationReducer';
import ratesReducer from '../modules/Main/dataReducers/ratesReducer';


// Persist Rate List
const persistRateListConfig = {
  key: 'rateListReducer',
  storage,
  stateReconciler: autoMergeLevel2,
  whitelist: [ 'filter', 'unfilteredRows', 'rows', 'timestampConfiguration', ],
};


/**
 * Reducer Tree
 */
export default combineReducers({
  data: combineReducers({
    ratesConfiguration: ratesConfigurationReducer,
    rates: ratesReducer,
  }),
  rateListPage: combineReducers({
    rateList: persistReducer(persistRateListConfig, rateListReducer),
  }),
});
