import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose, combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';

import { fetchConfigSaga } from './store/sagas';
import configReducer from './store/reducers/configReducer';
import ratesReducer from './store/reducers/ratesReducer';
import filterReducer from './store/reducers/filterReducer';
import App from './App';
import 'normalize.css';
import './index.css';

const composeEnhancers =
  process.env.NODE_ENV === 'development'
    ? window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__
    : null || compose;

const rootReducer = combineReducers({
  config: configReducer,
  rates: ratesReducer,
  filtered: filterReducer
});

const sagaMiddleware = createSagaMiddleware();

const store = createStore(
  rootReducer,
  composeEnhancers(applyMiddleware(sagaMiddleware))
);

sagaMiddleware.run(fetchConfigSaga);

const root = document.getElementById('exchange-rate-client');

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,

  root
);
