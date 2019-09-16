import React from 'react';
import ReactDOM from 'react-dom';
import { createBrowserHistory } from 'history'
import configureStore from './configureStore';
import Application from './Application';

const store = configureStore();
const history =  createBrowserHistory();

ReactDOM.render(
    <Application store={store} history={history} />,
    document.getElementById('exchange-rate-client')
);