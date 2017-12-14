import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import {createStore} from 'redux';
import {Provider} from 'react-redux';
import thunk from 'redux-thunk';
import {applyMiddleware} from 'redux';
import reducer from './model/reducer';

const store = createStore(reducer, applyMiddleware(thunk));

ReactDOM.render(<Provider store={store}>
    <App />
</Provider>, document.getElementById('root'));
