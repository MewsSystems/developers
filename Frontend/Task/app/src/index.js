import React from 'react';
import ReactDOM from 'react-dom';

//import { Provider } from 'react-redux'
import { createStore, applyMiddleware, compose, combineReducers } from 'redux'
import {StoreContext} from 'redux-react-hook';
import thunk from 'redux-thunk'

import pairsReducer from './store/reducers/pairs'
import ratesReducer from './store/reducers/rates'

import './index.css';
import App from './App';

const composeEnhancers = process.env.NODE_ENV === 'development' ? window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ : null || compose;

const rootReducer = combineReducers({
    pairs: pairsReducer,
    rates: ratesReducer,
})

const store = createStore(rootReducer, composeEnhancers(applyMiddleware(thunk)))

const app = (
    <StoreContext.Provider value={store}>
        <App />
    </StoreContext.Provider>
)

ReactDOM.render(app, document.getElementById('root'));
