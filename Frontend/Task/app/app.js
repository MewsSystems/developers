import React from 'react';
import ReactDOM from 'react-dom';
import configureStore from './configureStore';
import Application from './Application';

const store = configureStore();

ReactDOM.render(
    <Application store={store} />,
    document.getElementById('exchange-rate-client')
);