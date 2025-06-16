import { createGlobalStyle } from "styled-components";
import { colors } from "../tokens/colors";

export const GlobalStyle = createGlobalStyle`
    html {
        box-sizing: border-box;
    }
    
    *, :after, :before {
        box-sizing: inherit;
    }

    body {
        background-color: ${colors.bgPrimary};
        color: ${colors.textPrimary};
        font-family: 'Roboto', sans-serif;
        font-weight: 300;
        margin: 0;
        padding: 0;
    }
`