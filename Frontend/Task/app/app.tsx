import { endpoints, interval } from './config.json';
import { render } from 'react-dom';
import { Main } from './Main';
import * as React from 'react';
import { createGlobalStyle } from 'styled-components';

const GlobalStyle = createGlobalStyle`
  @import url('https://fonts.googleapis.com/css?family=Montserrat&display=swap');
  
  body {
    font-family: 'Montserrat', sans-serif;
    background: #fff;
    color: #000;
  }
`;

export function run(element: HTMLElement) {
  render(
    <>
      <GlobalStyle />
      <Main
        configUrl={endpoints.config}
        ratesUrl={endpoints.rates}
        ratesRefreshMilliseconds={interval}
      />
    </>,
    element,
  );
}
