import { createGlobalStyle } from 'styled-components';
import { Color } from './enums/style/color';
import { Spacer } from './enums/style/spacer';

// Possible to use theme provider for the shared styles like colors
export const GlobalStyle = createGlobalStyle`
    * {
        box-sizing: border-box;
    }

    body {
        margin: 0;
        padding: 0;
        font-weight: normal;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        color: ${Color.Primary};
        background-color: ${Color.Background};
        text-align: left;
    }

    body,
    button {
        // font-family based on - https://modernfontstacks.com/
        font-family: Bahnschrift, 'DIN Alternate', 'Franklin Gothic Medium', 'Nimbus Sans Narrow', sans-serif-condensed, sans-serif;
    }

    #root {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
    }

    a {
        color: ${Color.Accent};
    }

    h1, h2, h3, h4, h5, h6 {
        margin: ${Spacer.Sm} 0;
    }
`;