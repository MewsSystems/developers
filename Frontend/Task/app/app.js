import React, { useEffect } from "react";
import ReactDOM from "react-dom";
import { createStore, applyMiddleware, compose } from "redux";
import appReducer from "./reducers";
import { composeWithDevTools } from "redux-devtools-extension";
import { getPairs } from "./actions";
import thunk from "redux-thunk";
import { Provider } from "react-redux";
import Pairs from "./components/PairList";

const store = createStore(
    appReducer,
    compose(
        applyMiddleware(thunk),
        composeWithDevTools()
    )
);

const App = () => {
    useEffect(() => {
        store.dispatch(getPairs());
    }, []);

    return (
        <Provider store={store}>
            <h1>Mews Exchange Rate</h1>
            <div className="container-fluid">
                <div className="row">
                    <div className="col-3 p-2">
                        <Pairs />
                    </div>
                    <div className="col-9 p-2">
                    </div>
                </div>
            </div>
        </Provider>
    );
};

ReactDOM.render(<App />, document.getElementById("exchange-rate-client"));
