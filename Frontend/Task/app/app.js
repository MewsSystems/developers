// @flow strict

import * as React from 'react';
import ReactDOM from 'react-dom';

document.addEventListener('DOMContentLoaded', () => {
  const App = () => <span>Hello world!</span>;

  const appContainer = document.getElementById('exchange-rate-client');

  if (appContainer) {
    ReactDOM.render(<App />, appContainer);
  }
});
