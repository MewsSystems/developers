import { createGlobalStyle } from "styled-components";

export default createGlobalStyle`

  /* 
    Josh W Comeau's CSS reset
  */

  /*
    1. Use a more-intuitive box-sizing model.
  */
  *, *::before, *::after {
    box-sizing: border-box;
  }

  /*
    2. Remove default margin
  */
  * {
    margin: 0;
  }

  /*
    3. Allow percentage-based heights in the application
  */
  html, body {
    height: 100%;
  }

  /*
    Typographic tweaks!
    4. Add accessible line-height
    5. Improve text rendering
  */
  body {
    line-height: 1.5;
    -webkit-font-smoothing: antialiased;
  }

  /*
    6. Improve media defaults
  */
  img, picture, video, canvas, svg {
    display: block;
    max-width: 100%;
  }

  /*
    7. Remove built-in form typography styles
  */
  input, button, textarea, select {
    font: inherit;
  }

  /*
    8. Avoid text overflows
  */
  p, h1, h2, h3, h4, h5, h6 {
    overflow-wrap: break-word;
  }

  /*
    9. Create a root stacking context
  */
  #root, #__next {
    isolation: isolate;
  }

  /*
    Fontstack 
  */
  html {
    --font-sans-serif:
      -apple-system, BlinkMacSystemFont, avenir next, avenir, segoe ui,
      helvetica neue, helvetica, Ubuntu, roboto, noto, arial, sans-serif;

    font-family: var(--font-sans-serif);
  }

  /*
    UX bits
  */

  #root { 
    --color-dark-text: hsl(240, 10%, 3.92%);
    --color-light-text: hsl(240, 3.83%, 46.08%);
    --color-dark-gray: hsl(170, 0%, 38%);
    --color-light-gray: hsl(170, 0%, 86%);
  }
  
`;
