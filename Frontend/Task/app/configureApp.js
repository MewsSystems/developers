import React from 'react';
import { Provider } from 'react-redux';
import store from './configureStore';
//import configureStore from './configureStore';

//const store = configureStore();

export default () => (
    <Provider store={store}>
        <ExchangeRates />
    </Provider>
);