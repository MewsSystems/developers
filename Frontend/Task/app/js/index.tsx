import * as ReactDOM from 'react-dom';
import * as React from 'react';
import App from './App';
import { tryFetchConfig } from './helpers/fetchConfig';
import { Provider } from 'react-redux';
import store from './store/store';
import { fetchValues } from './helpers/fetchValues';
import { storeBackuper } from './helpers/storeBakcup';

storeBackuper();
tryFetchConfig();
fetchValues();

ReactDOM.render(
    <Provider store={store}>
        <App />
    </Provider>,
    document.getElementById('exchange-rate-client')
);