import { createGlobalStyle } from "styled-components"

export const GlobalStyle = createGlobalStyle`
*, *::before, *::after {
  box-sizing: border-box;
  margin: 0;
}

html,
body, #__next {
  padding: 0;
  height: 100%;
}

html {
  font-size: 62.5%;
}

body {
  font: 400 1.6rem 'sans-serif';
  text-rendering: optimizeLegibility;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  font-smooth: always;
}

a {
  color: inherit;
  text-decoration: none;
}

&:focus {
  outline: none;
}
`
