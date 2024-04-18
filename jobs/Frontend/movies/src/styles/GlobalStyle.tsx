import { createGlobalStyle } from "styled-components";

export const GlobalStyle = createGlobalStyle`
:root {
  --clr-white: 0 0% 100%;

  /* slate colors */

  --clr-slate-50: #f8fafc;
  --clr-slate-100: #f1f5f9;
  --clr-slate-200:#e2e8f0;
  --clr-slate-300: #cbd5e1;
  --clr-slate-400: #94a3b8;
  --clr-slate-500: #64748b;
  --clr-slate-600: #475569;
  --clr-slate-700: #334155;
  --clr-slate-800: #1e293b;
  --clr-slate-900: #0f172a;
  --clr-slate-950: #020617;

  /* blue colors */
  --clr-blue-50: #eff6ff;
  --clr-blue-100:#dbeafe;
  --clr-blue-200:#bfdbfe;
  --clr-blue-300:#93c5fd;
  --clr-blue-400:#60a5fa;
  --clr-blue-500:#3b82f6;
  --clr-blue-600:#2563eb;
  --clr-blue-700:#1d4ed8;
  --clr-blue-800:#1e40af;
  --clr-blue-900:#1e3a8a;
  --clr-blue-950:#1e1b4b;

  /* font sizes */
  --fs-900: 9.375rem;
  --fs-800: 6.25rem;
  --fs-700: 3.5rem;
  --fs-600: 2rem;
  --fs-500: 1.75rem;
  --fs-400: 1.125rem;
  --fs-300: 1rem;
  --fs-200: 0.875rem;

  /* font families */
  --ff-sans: ui-sans-serif, system-ui, sans-serif, "Apple Color Emoji", "Segoe UI Emoji";
  --ff-serif: ui-serif, Georgia, Cambria, "Times New Roman", Times, serif; 
}

* {
  min-width: 0;
  font: inherit; 
} 

*, *:: before, *:: after {
  box-sizing: border-box;
} 

img, video, svg {
  display: block;
  height: auto;
  max-width: 100%; 
} 
body,
h1,
h2,
h3,
h4,
h5,
figure,
picture {
  margin: 0;
}

h1,
h2,
h3,
h4,
h5,
h6,
p {
  font-weight: 400;
}


body {
  margin: 0;
  min-height: 100dvh; 
  background-color: var(--clr-slate-200);
  max-width: 72rem;
  margin: 0px auto;
} 

h1, h2, h3, h4, h5, h6 {
  text-wrap: balance;
} 

p {
  text-wrap: pretty;
}

`;
