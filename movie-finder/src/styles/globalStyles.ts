import { createGlobalStyle } from "styled-components";

export const GlobalStyle = createGlobalStyle`
  html{
    font-size: 16px;
    body {
      font-family: "Roboto", "Arial", sans-serif;
      margin: 0;
      padding: 0;
      border: 0;
      vertical-align: baseline;
      line-height: 1.2;
      height: 100vh;
    }
  }
  
  * {
    box-sizing: border-box;
  }
  button{
    font-size: 100%;
    font-family: inherit;
    border: 0;
    padding: 0;
  }
  
`;
