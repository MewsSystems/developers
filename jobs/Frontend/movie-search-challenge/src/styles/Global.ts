import { createGlobalStyle } from "styled-components"

export const GlobalStyles = createGlobalStyle`
/* @import url("https://fonts.googleapis.com/css?family=Montserrat:300,400,700,800"); */

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  background-color: ${({ theme }) => theme.colors.primary};
  font-family: 'Montserrat', sans-serif;
}
`
