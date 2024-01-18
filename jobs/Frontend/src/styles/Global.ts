import { Inter } from 'next/font/google'
import { createGlobalStyle } from 'styled-components';

const inter = Inter({ weight: ['400', '500', '600', '700'], subsets: ['latin-ext'], display: 'swap', adjustFontFallback: false });

const GlobalStyle = createGlobalStyle`
  :root {
    --font: ${inter.style.fontFamily}, -apple-system, ".SFNSText-Regular", "San Francisco", BlinkMacSystemFont, "Segoe UI", "Helvetica Neue", Helvetica, Arial, sans-serif;
  }
  
  html, body, div, span, applet, object, iframe,
  h1, h2, h3, h4, h5, h6, p, blockquote, pre,
  a, abbr, acronym, address, big, cite, code,
  del, dfn, em, img, ins, kbd, q, s, samp,
  small, strike, strong, sub, sup, tt, var,
  b, u, i, center,
  dl, dt, dd, menu, ol, ul, li,
  fieldset, form, label, legend,
  table, caption, tbody, tfoot, thead, tr, th, td,
  article, aside, canvas, details, embed,
  figure, figcaption, footer, header, hgroup,
  main, menu, nav, output, ruby, section, summary,
  time, mark, audio, video {
    margin: 0;
    padding: 0;
    border: 0;
    font-size: 100%;
    font: inherit;
    vertical-align: baseline;
  }
  /* HTML5 display-role reset for older browsers */
  article, aside, details, figcaption, figure,
  footer, header, hgroup, main, menu, nav, section {
    display: block;
  }
  /* HTML5 hidden-attribute fix for newer browsers */
  *[hidden] {
    display: none;
  }
  menu, ol, ul {
    list-style: none;
  }
  blockquote, q {
    quotes: none;
  }
  blockquote:before, blockquote:after,
  q:before, q:after {
    content: '';
    content: none;
  }
  table {
    border-collapse: collapse;
    border-spacing: 0;
  }

  i {
    font-style: italic;
  }

  html {
    box-sizing: border-box;
    font-size: 15px;
  }

  *, *:before, *:after {
    box-sizing: inherit;
  }

  html, body {
    font-family: var(--font);
    color: var(--black);
  }

  body {
    font-weight: 500;
    line-height: 1.6;
  }

  button {
    font-family: var(--font);
    font-size: 15px;
  }

  input,
  select,
  textarea {
    font-family: var(--font);
  }

  strong {
    font-weight: bold;
  }

`;

export default GlobalStyle;
