import styled, { createGlobalStyle } from 'styled-components';

export const GlobalStyle = createGlobalStyle`
  :root {
    --common-color-light: #F8F6E3;
    --common-color-blue-1: #97E7E1;
    --common-color-blue-2: #6AD4DD;
    --common-color-blue-3: #7AA2E3;
    --common-color-blue-4: #558AE0;
    --common-color-error: #FFAF45;
    --common-medium-size: 400px;
  }

  * {
    box-sizing: border-box;
  }
`;

export const AppWrapper = styled.div`
  width: 1000px;
  max-width: 100%;
  min-height: 100vh;
  background-color: var(--common-color-light);
  margin: 0 auto;
  font-family: 'Work Sans', sans-serif;
`;