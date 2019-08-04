// @flow strict

import * as React from 'react';
import ReactDOM from 'react-dom';

import App from '.';

document.addEventListener('DOMContentLoaded', () => {
  const appContainer = document.getElementById('exchange-rate-client');

  if (appContainer) {
    ReactDOM.render(<App />, appContainer);
  }
});
