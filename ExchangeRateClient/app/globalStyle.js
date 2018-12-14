import { createGlobalStyle } from "styled-components";

const GlobalStyle = createGlobalStyle`
  @import url('https://fonts.googleapis.com/css?family=Poppins:400,500,700,800');

  body {
    margin: 0;
    padding: 0;
    font-family: 'Poppins', sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
  }

  input, textarea, select, button {
    font-family: inherit;
  }

  h1, h2 {
    margin: 0;
  }
`;

export default GlobalStyle;
