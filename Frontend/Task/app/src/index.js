// @flow

import * as React from 'react';
import { Provider } from 'react-redux';
import { createGlobalStyle } from 'styled-components';
import reset from 'styled-reset';

import store from './redux/store';
import View from './view';

const GlobalStyle = createGlobalStyle`
  ${reset}
  font-family: 'Roboto', sans-serif;`;

const App = () => (
  <Provider store={store}>
    <View />
    <GlobalStyle />
  </Provider>
);

export default App;
