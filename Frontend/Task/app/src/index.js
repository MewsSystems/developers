import React from 'react';
import ReactDOM from 'react-dom';
import App from './components/App';

import {createStore} from 'redux';
import {Provider} from 'react-redux';
import combinedReducers from './reducers/combinedReducers';

import {persistStore, persistReducer} from 'redux-persist';
import {PersistGate} from 'redux-persist/integration/react';
import storage from 'redux-persist/lib/storage';

const persistConfig = {
  key: 'reduxstorage',
  storage,
};
const persistedReducer = persistReducer (persistConfig, combinedReducers);

const store = createStore (persistedReducer);
const persistor = persistStore (store);
store.subscribe (state => console.log ('Redux store changed: ', state));

ReactDOM.render (
  <Provider store={store}>
    <PersistGate
      loading={
        <div className="loader-container">
          <div className="loader" />
        </div>
      }
      persistor={persistor}
    >
      <App />
    </PersistGate>
  </Provider>,
  document.getElementById ('root')
);
