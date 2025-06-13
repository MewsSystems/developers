import { createGlobalStyle } from "styled-components";

export const GlobalStyle = createGlobalStyle`
    html {
        box-sizing: border-box;
    }
    
    *, :after, :before {
        box-sizing: inherit;
    }

    body {
        background-color: #000;
        color: white;
        font-family: 'Roboto', sans-serif;
        font-weight: 300;
        margin: 0;
        padding: 0;
    }
`