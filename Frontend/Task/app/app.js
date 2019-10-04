import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import reduxThunk from 'redux-thunk';

import reducers, { INITIAL_STATE } from './reducers';
import ErrorBoundary from './components/ErrorBoundary'; // top level error boundary
import ExchangeRateContainer from './containers/ExchangeRateContainer'; // main container
import './app.css'; // global CSS

// try to load configuration and filter settings from localStorage
const loadSettingsFromLocalStorage = () => {
  try {
    const stringifiedSettings = localStorage.getItem('ExchangeRateSettings');
    if (!stringifiedSettings) return undefined; // no previous settings available
    // const loadedSettings = JSON.parse(stringifiedSettings);
    const {
      currencyPairs,
      filter,
      selectedCurrencyPairs,
    } = JSON.parse(stringifiedSettings);
    return {
      ...INITIAL_STATE,
      currencyPairs,
      filter,
      selectedCurrencyPairs,
    };
  } catch {
    return undefined;
    // ignore errors so application continues to function without loading previous settings
  }
};

const store = createStore(
  reducers,
  loadSettingsFromLocalStorage(),
  compose(applyMiddleware(reduxThunk)),
);

render(
  <Provider store={store}>
    <ErrorBoundary>
      <ExchangeRateContainer />
    </ErrorBoundary>
  </Provider>,
  document.getElementById('exchange-rate-client'),
);
