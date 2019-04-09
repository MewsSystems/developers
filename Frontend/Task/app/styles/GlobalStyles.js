import { createGlobalStyle } from 'styled-components'
import { theme } from './theme'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faArrowUp,
  faArrowDown,
  faArrowsAltH,
} from '@fortawesome/free-solid-svg-icons'

library.add(faArrowUp)
library.add(faArrowDown)
library.add(faArrowsAltH)

export const GlobalStyles = createGlobalStyle`
  @font-face {
    font-family: 'circular';
    src: url('font/CircularStd-Book.otf') format ('opentype');
    font-weight: 200;
  }

  * {
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;

    &::after,
    &::before {
      box-sizing: border-box;
    }
  }

  html {
    font-size: 62.5%;
  }

  body {
    font-size: 1.6rem;
    font-family: 'circular', sans-serif;
    font-weight: 200;
    background-color: ${theme.color.softGray};
  }

  #root {
    width: 100%;
    height: 100%;
  }
`
