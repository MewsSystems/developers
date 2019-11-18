import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import store from './redux/store';
import { loadCurrencyConfiguration } from './redux/actions/currency';

import App from './app';

store.dispatch(loadCurrencyConfiguration());

const AppContainer = () => (
  <Provider store={store}>
    <App/>
  </Provider>
);

ReactDOM.render(<AppContainer />, document.getElementById('root'));
