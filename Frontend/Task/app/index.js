// @flow

import * as React from 'react';
import { Provider } from 'react-redux';

import store from './redux/store';

const App = () => (
  <Provider store={store}>
    <span>hello!</span>
  </Provider>
);

export default App;
