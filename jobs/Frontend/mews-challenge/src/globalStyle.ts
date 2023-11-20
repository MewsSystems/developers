import { createGlobalStyle } from 'styled-components';
import { Color } from './app/types';
 
const GlobalStyle = createGlobalStyle`
  body {
    margin: 0;
    padding: 0;
    background: ${Color.background};
    font-family: Open-Sans, Helvetica, Sans-Serif;
    overflow:auto;
  }

  @keyframes spin {
    from {
        transform: scale(1) rotate(0deg);
    }
    to {
        transform: scale(1) rotate(360deg);
    }
}
`;
 
export default GlobalStyle;