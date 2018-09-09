// @flow strict

import React, { Component } from "react";
import styled from "styled-components";
import { Provider } from "react-redux";

import { ThemeProvider } from "styled-components";
import createStore from "./store/createStore";
import GlobalStyle from "./theme/Global";

import theme from "./theme";

const Root = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: ${({ theme }) => theme.colors.color1};
`;

const Title = styled.h1`
  color: ${({ theme }) => theme.colors.color2};
`;

type Props = {
  data: Array<string>,
};
class App extends Component<{}> {
  render() {
    return (
      <Provider store={createStore()}>
        <ThemeProvider theme={theme}>
          <React.Fragment>
            <Root className="App">
              <header className="App-header">
                <Title>Welcome to Exchange Rate App</Title>
              </header>
              <p className="App-intro">
                To get started, edit <code>src/App.js</code> and save to reload.
              </p>
            </Root>
            <GlobalStyle />
          </React.Fragment>
        </ThemeProvider>
      </Provider>
    );
  }
}

export default App;
