import { createStore, combineReducers } from 'redux'
import { persistStore, persistReducer } from 'redux-persist'
import storage from 'redux-persist/lib/storage'

import config from './config'
import rates from './rates'
import pairs from './pairs'

const reducer = combineReducers({
  config,
  rates,
  pairs,
})

const persistConfig = {
  key: 'app',
  storage,
  blacklist: ['rates'],
}

const persistedReducer = persistReducer(persistConfig, reducer)

export const store = createStore(persistedReducer)
export const persistor = persistStore(store)
