// @flow strict

import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";
import throttle from "lodash.throttle";
import rootReducer from "./rootReducer";
import type { StateTypes } from "./types";

export default function configureStore(persistedState: StateTypes) {
  return createStore(rootReducer, persistedState, composeWithDevTools(applyMiddleware(thunk)));
}
