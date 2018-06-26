import React from 'react';
import App from './components/App';
import { Provider } from 'react-redux';
import configureStore from './configureStore';

const store = configureStore();

export default () => (
    <Provider store={store}>
        <App />
    </Provider>
);
