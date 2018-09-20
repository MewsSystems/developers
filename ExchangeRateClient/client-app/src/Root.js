// @flow strict

import React, { Component } from "react";

import { Provider } from "react-redux";

import { ThemeProvider } from "styled-components";

import type { Store as ReduxStore, Dispatch as ReduxDispatch } from "redux";
import Select from "./components/Select";
import createStore from "./store/createStore";
import GlobalStyle from "./theme/Global";

import App from "./App";
import type { StateTypes } from "./store/types";
import type { Action } from "./actions/index";
import theme from "./theme";

type Props = { store: ReduxStore<StateTypes, Action> };
class Root extends Component<Props> {
  render() {
    return (
      <Provider store={this.props.store}>
        <ThemeProvider theme={theme}>
          <React.Fragment>
            <App />
            <GlobalStyle />
          </React.Fragment>
        </ThemeProvider>
      </Provider>
    );
  }
}

export default Root;
