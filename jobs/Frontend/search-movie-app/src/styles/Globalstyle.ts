import { createGlobalStyle } from 'styled-components';

export const GlobalStyle = createGlobalStyle`
    body {    
        background-color: ${({ theme }) => theme.colors.background};
        color: ${({ theme }) => theme.colors.onSurface};
        transition: background-color 0.2s ease-in-out, color 0.2s ease-in-out;
        margin: 0;
        display: flex;
        place-items: center;
        min-width: 320px;
        font-synthesis: none;
        text-rendering: optimizeLegibility;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        max-width: 1280px;
        margin: 0 auto;
        padding: 2rem;
        text-align: center;
    }`;
