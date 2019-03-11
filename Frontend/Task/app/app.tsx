import React from 'react';
import ReactDOM from 'react-dom';
import { applyMiddleware, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import { ExchangeInterface } from './interface/exchangeInterface';
import ExchangeReducer from './reducer/exchange';
import { getPairCollectionWithShortcut } from './action/currencyPair';
import CurrencyPairWrapper from './container/currencyPairWrapper';
import { GlobalStyle, WrapperStyle } from './style/wrapper';

const initialState: ExchangeInterface = {
    loading: false,
    error: false,
    selectedPair: localStorage.getItem('pairId'),
};

const store = createStore(
    ExchangeReducer,
    initialState,
    compose(
        applyMiddleware(thunk),
        (window as any).__REDUX_DEVTOOLS_EXTENSION__ && (window as any).__REDUX_DEVTOOLS_EXTENSION__(),
    ),
);

const App = () => (
    <Provider store={store}>
        <GlobalStyle />
        <WrapperStyle>
            <h1>Currency Pairs</h1>
            <CurrencyPairWrapper  />
        </WrapperStyle>
    </Provider>
);

ReactDOM.render(
    <App />,
    document.getElementById('exchange-rate-client'),
);

