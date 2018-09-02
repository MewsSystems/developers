import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { App } from './containers';
import { store, persistor } from './store';

const Root = () =>
    <Provider store={store} >
        <PersistGate loading={null} persistor={persistor}>
            <App />
        </PersistGate>
    </Provider>;

const app = document.querySelector('#exchange-rate-client');
ReactDOM.render(<Root />, app);
