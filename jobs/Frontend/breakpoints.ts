const breakpoints = {
  tabletBreakpoint: 768,
  desktopBreakpoint: 1024,
};

export const tabletMediaQuery = () =>
  `@media (min-width: ${breakpoints.tabletBreakpoint}px)`;

export const desktopMediaQuery = () =>
  `@media (min-width: ${breakpoints.desktopBreakpoint}px)`;
