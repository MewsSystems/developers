import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from './store';
import Layout from './modules/core/Layout';

export default function App() {
    return (
        <Provider store={store}>
            <Router>
                <Layout />
            </Router>
        </Provider>
    );
}
