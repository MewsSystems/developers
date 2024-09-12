import { createGlobalStyle } from "styled-components"

export const GlobalStyles = createGlobalStyle`
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
