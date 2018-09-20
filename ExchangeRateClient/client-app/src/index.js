// @flow strict
import React from "react";
import ReactDOM from "react-dom";
import throttle from "lodash.throttle";
import Root from "./Root";
import registerServiceWorker from "./registerServiceWorker";
import createStore from "./store/createStore";
import { loadState, saveState } from "./localStorage";

const persistedState = loadState();

const store = createStore(persistedState);

store.subscribe(
  throttle(() => {
    const { APIerror, isFetchingConfig, isFetchingRates, ...state } = store.getState();
    saveState(state);
  }, 1000),
);
const root = document.getElementById("root");
if (root !== null) {
  ReactDOM.render(<Root store={store} />, root);
} else {
  throw new Error("missing root element");
}

registerServiceWorker();
