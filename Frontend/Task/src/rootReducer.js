import { combineReducers } from 'redux';
import { persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import rates from './features/Rates/reducer';

const rootPersistConfig = {
  key: 'root',
  storage,
  blacklist: ['rates'],
};

const ratesPersistConfig = {
  key: 'rates',
  storage,
  whitelist: ['currencyPairs', 'selected'],
};

const rootReducer = combineReducers({
  rates: persistReducer(ratesPersistConfig, rates),
});

export default persistReducer(rootPersistConfig, rootReducer);
