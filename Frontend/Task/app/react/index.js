import React from 'react';
import { render } from 'react-dom';
import '@babel/polyfill';

import configureStore from './configurateStore';
import RootComponent from './components/RootComponent';

const initialState = window.__INITIAL_STATE__;
const store = configureStore({ initialState });
const rootElement = document.getElementById('reactRoot');

if (process.env.NODE_ENV !== 'production') {
  window.store = store;
}

render(
<RootComponent store={store} />,
rootElement
);

if (module.hot) {
  module.hot.accept('./components/RootComponent', () => {
    const NextRootComponent = require('./components/RootComponent').default;
    render(
      <NextRootComponent store={store}/>,
      rootElement
    );
  });
}
