import React from 'react';
import ReactDOM from 'react-dom';

import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';

import { configureStore, getPersistor } from './configureStore';

import config from './config';
import './index.css';
import App from './App';

const preloadedState = {};
const env = {
  NODE_ENV: process.env.NODE_ENV,
};

const store = configureStore(preloadedState, config, env);
const persistor = getPersistor(store);

ReactDOM.render(
  <Provider store={store}>
    <PersistGate loading={null} persistor={persistor}>
      <App config={config} />
    </PersistGate>
  </Provider>,
  document.getElementById('root'),
);
