// @flow
import React from 'react';
import { Provider } from 'react-redux';
import { injectGlobal } from 'styled-components';

import configureStore from './configureStore';
import Home from './Home';

injectGlobal`
  body {
    height: 100vh;
    margin: 0;
    padding: 0;
    font: 400 16px/1 ".SF NS Text", sans-serif;
    color: black;
  }
`;

const store = configureStore();
const Root = () => (
  <Provider store={store}>
    <Home />
  </Provider>
);

export default Root;
