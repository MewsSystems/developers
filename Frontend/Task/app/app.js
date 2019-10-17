import React, { useEffect } from "react";
import ReactDOM from "react-dom";
import { createStore, applyMiddleware, compose } from "redux";
import appReducer from "./reducers";
import { composeWithDevTools } from "redux-devtools-extension";
import { getRates } from "./actions";
import thunk from "redux-thunk";

const store = createStore(
    appReducer,
    compose(
        applyMiddleware(thunk),
        composeWithDevTools()
    )
);

const App = () => {
    useEffect(() => {
        store.dispatch(getRates());
    }, []);

    return <p>exchange rate</p>;
};

ReactDOM.render(<App />, document.getElementById("exchange-rate-client"));
