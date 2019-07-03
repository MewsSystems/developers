import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose, combineReducers } from 'redux';
import createSagaMiddleware from 'redux-saga';

import { fetchData } from './store/sagas';
import rootReducer from './store/reducers/';
import App from './App';
import 'normalize.css';
import './index.css';

const composeEnhancers =
  process.env.NODE_ENV === 'development'
    ? (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__
    : null || compose;

const sagaMiddleware = createSagaMiddleware();

const store = createStore(
  rootReducer,
  composeEnhancers(applyMiddleware(sagaMiddleware))
);

sagaMiddleware.run(fetchData);

const root = document.getElementById('exchange-rate-client');

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  root
);
