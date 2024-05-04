"use client";

import { createGlobalStyle } from "styled-components";

const GlobalStyles = createGlobalStyle`
  body {
    color: ${(props) => props.theme.primary.text};
    background-color: ${(props) => props.theme.primary.background};
  }
`;

export default GlobalStyles;
