import React from 'react';
import { Provider } from 'react-redux';
import store from './configureStore';
import App from './components/App';

export default () => (
    <Provider store={store}>
        <App />
    </Provider>
);
