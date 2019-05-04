import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import reduxThunk from 'redux-thunk';
import reducers from './src/reducers';
import App from './src/App';

// eslint-disable-next-line no-underscore-dangle
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
const store = createStore(
  reducers, /* preloadedState, */
  composeEnhancers(
    applyMiddleware(reduxThunk),
  ),
);

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>, document.getElementById('exchange-rate-client'),
);
