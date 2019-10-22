import React from 'react';
import ReactDOM from 'react-dom';

import { createStore, applyMiddleware, compose, combineReducers } from 'redux'
import { initialStatePairs } from "./store/reducers/pairs";
import { initialStateRates } from "./store/reducers/rates";

import { Provider } from 'react-redux';
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

const initialState = { initialStatePairs, initialStateRates}

const loadSessionStorage = () => {
    if (sessionStorage.length > 0) {

        const pairs = JSON.parse(sessionStorage.getItem('pairs'))
        const rates = JSON.parse(sessionStorage.getItem('rates'))
        const pairsLinks = JSON.parse(sessionStorage.getItem('pairsLinks'))
        const allRates = JSON.parse(sessionStorage.getItem('allRates'))

        return {
            pairs: {
                pairs: pairs ? pairs : [],
                pairsLinks: pairsLinks ? pairsLinks : []
            },
            rates: {
                rates: rates ? rates : [],
                allRates: allRates ? allRates : []
            },
        };
    } else {
        return initialState
    }
};


const store = createStore(rootReducer, loadSessionStorage(), composeEnhancers(applyMiddleware(thunk)))

const app = (
    <Provider store={store}>
        <App />
    </Provider>
)

ReactDOM.render(app, document.getElementById('root'));
