import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import * as serviceWorker from './serviceWorker';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux';
import Reducer from './store/reducer';
import { fetchConfiguration } from './store/apiCalls'
import thunk from 'redux-thunk';
// import updateMessagesMiddleware from './store/apiCalls'
const appStore = createStore(Reducer, applyMiddleware(thunk));

appStore.dispatch(fetchConfiguration());

ReactDOM.render(<Provider store={appStore}><App /></Provider>, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
