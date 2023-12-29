// import original module declarations
import 'styled-components'

export type Theme = {
  colors: {
    text: {
      primary: string
      secondary: string
    }
    border: {
      primary: string
    }
    fill: {
      primary: string
      secondary: string
    }
  }
  sizes: {
    primaryHeading: string
    secondaryHeading: string
    primarySpan: string
    secondarySpan: string
    tertiarySpan: string
  }
}

declare module 'styled-components' {
  // Extend default theme with custom theme
  export interface DefaultTheme extends Theme {}
}
