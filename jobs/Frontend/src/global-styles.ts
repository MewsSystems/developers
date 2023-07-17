import { createGlobalStyle } from "styled-components";

export const GlobalStyles = createGlobalStyle`
  body, html, #root {
    height: 100%;
    width: 100%;
    font-family: sans-serif;
    padding: 0;
    margin: 0;
  }

  *, *:after, *:before {
    box-sizing: border-box;
  }
`;
