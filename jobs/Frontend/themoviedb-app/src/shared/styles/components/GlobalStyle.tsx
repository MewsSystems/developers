import { createGlobalStyle } from 'styled-components';

const GlobalStyle = createGlobalStyle`
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }

    body {
        background-color: ${(props) => props.theme.colors.bg};
        color: ${(props) => props.theme.colors.text};
        font-size: ${(props) => props.theme.fontSize.global};
        height: 100vh;
    }

    header, main, footer {
        width: 100%;
        max-width: 1280px;
        padding: 0% 10%;
        margin: 0 auto;
    }

    header, footer {
        padding-top: 32px;
        padding-bottom: 32px;
    }

    footer {
        text-align: center;
    }
`;

export default GlobalStyle;
