import React from 'react';
import { Provider }  from 'react-redux';
import { render } from 'react-dom';
import configureStore from './src/store/configureStore';
import App from './src/App';

import { endpoint, interval } from './config';

const store = configureStore();

export function run(element) {
    console.log('App is running.');
    render(
      <Provider store={store}>
        <App />
    </Provider>,
    document.getElementById(element)
  );
}
