// @flow strict

import { createStore, applyMiddleware, compose } from "redux";
import thunk from "redux-thunk";
import rootReducer from "./rootReducer";
import type { StateTypes } from "./types";

//  eslint-disable-next-line no-underscore-dangle
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export default function configureStore(persistedState?: StateTypes) {
  return createStore(rootReducer, persistedState, composeEnhancers(applyMiddleware(thunk)));
}
