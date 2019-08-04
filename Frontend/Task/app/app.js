// @flow strict

import * as React from 'react';
import ReactDOM from 'react-dom';
import 'core-js/stable';
import 'regenerator-runtime/runtime';

import App from './src';

document.addEventListener('DOMContentLoaded', () => {
  const appContainer = document.getElementById('exchange-rate-client');

  if (appContainer) {
    ReactDOM.render(<App />, appContainer);
  }
});
