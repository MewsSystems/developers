import React from 'react';
import ReactDOM from 'react-dom';

import { Provider } from 'react-redux';
import configureStore from './configureStore';

import config from './config';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

const preloadedState = {};
const env = {
  NODE_ENV: process.env.NODE_ENV,
};
const store = configureStore(preloadedState, config, env);

ReactDOM.render(
  <Provider store={store}>
    <App config={config} />
  </Provider>,
  document.getElementById('root'),
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();
