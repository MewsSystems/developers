import thunk from 'redux-thunk'
import storage from 'redux-persist/lib/storage'
import { applyMiddleware, compose, createStore } from 'redux'
import { persistStore, persistReducer } from 'redux-persist'

import rootReducer from '../reducers'

const persistConfig = {
  key: 'root',
  storage,
  blacklist: ['rateReducer'],
}
const persistedReducer = persistReducer(persistConfig, rootReducer)

const composeEnhancers =
  (typeof window !== 'undefined' &&
    window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) ||
  compose

const store = createStore(
  persistedReducer,
  composeEnhancers(applyMiddleware(thunk))
)
const persistor = persistStore(store)

export { store, persistor }
