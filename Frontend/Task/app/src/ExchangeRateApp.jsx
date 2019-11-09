import React from 'react';
import {createStore, applyMiddleware, compose} from "redux";
import {Provider} from "react-redux";
import reducer from './reducers/index'
import MainContainer from "./containers/MainContainer";
import thunk from "redux-thunk";

const store = createStore(
    reducer,
    compose(
        applyMiddleware(thunk),
        window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
    )
);

const ExchangeRateApp = () => {
    return (
        <Provider store={store}>
            <MainContainer/>
        </Provider>
    );
};

export default ExchangeRateApp;