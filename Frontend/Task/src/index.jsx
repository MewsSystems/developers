import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import store from './store.js';
import Application from './containers/application';

import './assets/styles/base.scss';

render(
  <Provider store={store}>
    <Application />
  </Provider>,
  document.getElementById('root'),
);
