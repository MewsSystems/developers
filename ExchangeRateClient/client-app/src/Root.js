// @flow strict

import React from "react";
import { Provider } from "react-redux";
import { ThemeProvider } from "styled-components";
import type { Store as ReduxStore } from "redux";
import GlobalStyle from "./theme/Global";
import App from "./App";
import type { StateTypes } from "./store/types";
import type { Action } from "./actions/index";
import theme from "./theme";

type Props = { store: ReduxStore<StateTypes, Action> };

const Root = ({ store }: Props) => (
  <Provider store={store}>
    <ThemeProvider theme={theme}>
      <React.Fragment>
        <App />
        <GlobalStyle />
      </React.Fragment>
    </ThemeProvider>
  </Provider>
);

export default Root;
