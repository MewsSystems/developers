import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import store from './store/store'
import './index.css';
import Base from './components/Base'
import * as serviceWorker from './serviceWorker';

const rootElement = document.getElementById('root');
ReactDOM.render(
    <Provider store={store}>
        <Base />
    </Provider>,
    rootElement
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();






