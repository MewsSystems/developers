import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import reduxThunk from 'redux-thunk';

import reducers from './reducers';
import ErrorBoundary from './components/ErrorBoundary'; // top level error boundary
import ExchangeRateContainer from './containers/ExchangeRateContainer'; // main container
import './app.css'; // global CSS

const store = createStore(
  reducers,
  undefined,
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
