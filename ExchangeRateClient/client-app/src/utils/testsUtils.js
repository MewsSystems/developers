// @flow
/* eslint-disable import/no-extraneous-dependencies */
import * as React from "react";

import { Provider } from "react-redux";
import { render } from "react-testing-library";
import { createStore } from "redux";
import rootReducer from "../store/rootReducer";

export default function renderWithRedux(
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
