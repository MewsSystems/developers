import React, { useEffect } from "react";
import ReactDOM from "react-dom";
import { createStore, applyMiddleware, compose } from "redux";
import appReducer from "./store/reducers";
import { composeWithDevTools } from "redux-devtools-extension";
import { getPairs, pollRates, persistChanges } from "./store/middleware";
import thunk from "redux-thunk";
import { Provider } from "react-redux";
import Pairs from "./components/PairList";
import Rates from "./components/RateList";
import { endpoint, interval } from "./config.json";

const [initialState, store] = (() => {
    const persistedData = localStorage["exchange"];
    const initialState = persistedData ? JSON.parse(persistedData) : undefined;
    const store = createStore(appReducer, initialState, compose(applyMiddleware(thunk, persistChanges), composeWithDevTools()));

    return [initialState, store];
})();

const App = () => {
    useEffect(() => {
        if (!initialState) {
            store.dispatch(getPairs());
        }
        const handler = setInterval(() => {
            store.dispatch(pollRates(endpoint));
        }, interval);

        return () => clearInterval(handler);
    }, []);

    return (
        <div className="App">
            <Provider store={store}>
                <h1>Mews Exchange Rate</h1>
                <div className="container-fluid">
                    <div className="row">
                        <div className="col-3 p-2">
                            <Pairs />
                        </div>
                        <div className="col-9 p-2">
                            <Rates />
                        </div>
                    </div>
                </div>
            </Provider>
        </div>
    );
};

ReactDOM.render(<App />, document.getElementById("exchange-rate-client"));
