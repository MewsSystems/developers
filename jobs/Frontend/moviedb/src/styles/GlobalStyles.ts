import { createGlobalStyle } from 'styled-components';

const GlobalStyles = createGlobalStyle`
   *, 
   *::before, 
   *::after {
      box-sizing: border-box;
   }

   html {
      font-family: 'Open Sans', Arial, Helvetica, sans-serif;
      font-size: 16px;
   }

   body, h1, h2, h3, h4, h5, h6, p, ol, ul {
      margin: 0;
      padding: 0;
   }

   ol, 
   ul {
      list-style: none;
   }

   img {
      max-width: 100%;
      height: auto;
   }
   
   h1 {
     font-size: 60px;
     font-weight: 700;
     margin-bottom: 1em;
     
     @media (max-width: 767px) {
       font-size: 40px;
     }
   }
`;

export default GlobalStyles;
