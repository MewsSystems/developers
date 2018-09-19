// @flow
import * as React from "react";

import { Provider, connect } from "react-redux";
import { render } from "react-testing-library";
import { createStore } from "redux";
import rootReducer from "../store/rootReducer";

export function renderWithRedux(
  ui: React.Node,
  {
    initialState,
    store = createStore(rootReducer, initialState),
  }: { initialState?: Object, store?: Object } = {},
) {
  return {
    ...render(<Provider store={store}>{ui}</Provider>),
    store,
  };
}
