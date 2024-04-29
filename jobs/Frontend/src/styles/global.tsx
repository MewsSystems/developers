import { createGlobalStyle } from 'styled-components';

const GlobalStyles = createGlobalStyle`
  :root {
    --bg-gray-dark: #333333;
  }

  :root {
    --spacing-4: 4px;
    --spacing-8: 8px;
    --spacing-16: 16px;
    --spacing-32: 32px;
    --spacing-40: 40px;
  }

  body {
    line-height: 0;
  }

  .d-flex {
    display: flex;
  }

  .d-justify-content-center {
    justify-content: center;
  }

  .d-align-items-center {
    align-items: center;
  }

  .p-t-4 {
    padding-top: var(--spacing-4);
  }

  .p-b-4 {
    padding-bottom: var(--spacing-4);
  }

  .p-t-8 {
    padding-top: var(--spacing-8);
  }

  .p-b-8 {
    padding-bottom: var(--spacing-8);
  }

  .p-t-16 {
    padding-top: var(--spacing-16);
  }

  .p-b-16 {
    padding-bottom: var(--spacing-16);
  }

  .p-40 {
    padding: var(--spacing-40);
  }

  .m-b-4 {
    margin-bottom: var(--spacing-4);
  }

  .m-b-8 {
    margin-bottom: var(--spacing-8);
  }

  .line-clamp-4 {
    overflow: hidden;
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 4;
  }

`;

export default GlobalStyles;
