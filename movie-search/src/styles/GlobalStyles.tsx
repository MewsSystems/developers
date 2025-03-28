import { createGlobalStyle } from "styled-components";

export const GlobalStyles = createGlobalStyle`
    *, *::before, *::after {
        box-sizing: border-box;
        margin: 0;
        padding: 0;
    }

    body, h1, h2, h3, h4, h5, h6, p, figure, blockquote, dl, dd {
        margin: 0;
    }

    ul, ol {
        list-style: none;
        padding: 0;
    }

    a {
        text-decoration: none;
        color: inherit;
    }

    button {
        background: none;
        border: none;
        cursor: pointer;
    }

    body {
        font-family: 'Inter', sans-serif;
        line-height: 1.5;
        color: #333;
    }

    img {
        max-width: 100%;
        height: auto;
    }

    :focus {
        outline: none;
    }
`