// @flow strict
import React from "react";
import ReactDOM from "react-dom";
import Root from "./Root";
import registerServiceWorker from "./registerServiceWorker";
import createStore from "./store/createStore";

const store = createStore();
const root = document.getElementById("root");
if (root !== null) {
  ReactDOM.render(<Root store={store} />, root);
} else {
  throw new Error("missing root element");
}

registerServiceWorker();
