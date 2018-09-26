// @flow strict
import React from "react";
import ReactDOM from "react-dom";
import throttle from "lodash.throttle";
import Omit from "lodash.omit";
import Root from "./Root";
import registerServiceWorker from "./registerServiceWorker";
import createStore from "./store/createStore";
import { loadState, saveState } from "./localStorage";
import { initalState } from "./store/rootReducer";

const persistedState = loadState();

const store = createStore(persistedState);

store.subscribe(
  throttle(() => {
    saveState({ ...Omit(store.getState(), ["isFetchingRates", "isFetchingConfig", "APIerror"]) });
  }, 1000),
);
const root = document.getElementById("root");
if (root !== null) {
  ReactDOM.render(<Root store={store} />, root);
} else {
  throw new Error("missing root element");
}

registerServiceWorker();
