import { createGlobalStyle } from 'styled-components'

const GlobalStyle = createGlobalStyle`
  @import url('https://fonts.googleapis.com/css?family=Open+Sans:300,400,500,600,700');

  * {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
    font-family: 'Open Sans', sans-serif;
    border: 0;
  }

  html {
    width: 100%;
    min-height: 100vh;
  }
`

export default GlobalStyle
