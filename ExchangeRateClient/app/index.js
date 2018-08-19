import React from "react";
import Main from "./components/Main";
import store from "./store";
import { Provider } from "react-redux";

export default () => (
  <Provider store={store}>
    <Main />
  </Provider>
);
