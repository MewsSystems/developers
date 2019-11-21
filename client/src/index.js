import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

import {createStore} from 'redux';
import {Provider} from 'react-redux';
import combinedReducers from './reducers/combinedReducers';

import {persistStore, persistReducer} from 'redux-persist';
import {PersistGate} from 'redux-persist/integration/react';
import storage from 'redux-persist/lib/storage';
import {Loader} from './ui';

const persistConfig = {
  key: 'ppp',
  storage,
};
const persistedReducer = persistReducer (persistConfig, combinedReducers);

const store = createStore (persistedReducer);
const persistor = persistStore (store);

if (process.env.NODE_ENV !== 'production') {
  store.subscribe (() => console.log ('State:', store.getState ()));
}

ReactDOM.render (
  <Provider store={store}>
    <PersistGate loading={<Loader />} persistor={persistor}>
      <App />
    </PersistGate>
  </Provider>,
  document.getElementById ('root')
);
