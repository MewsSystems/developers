import { applyMiddleware, compose, createStore } from 'redux';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import reduxThunk from 'redux-thunk';
import reducers from './reducers';

const persistConfig = {
  key: 'root',
  storage,
  blacklist: ['rates'],
};
const persistedReducer = persistReducer(persistConfig, reducers);

// eslint-disable-next-line no-underscore-dangle
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export default () => {
  const store = createStore(
    persistedReducer, /* preloadedState, */
    composeEnhancers(
      applyMiddleware(reduxThunk),
    ),

  );
  const persistor = persistStore(store);
  return { store, persistor };
};
