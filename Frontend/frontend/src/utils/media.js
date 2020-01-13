import { css, } from 'styled-components';


/**
 * Default media setup
 */
const defaultBreakpoints = {
  UHD: 3400,
  QHDPlus: 2900,
  QHD: 2300,
  FHD: 1750,
  HDPlus: 1500,
  HD: 1200,
  LG: 992,
  MD: 768,
  SM: 576,
  XS: 576,
};


const getBreakpoints = (props) => {
  if (props.theme && props.theme.grid && props.theme.grid.breakpoints) {
    return props.theme.grid.breakpoints;
  }
  return defaultBreakpoints;
};


/**
 * Media widths
 */
const media = Object.keys(defaultBreakpoints).reduce((accumulator, label) => {
  if (label === 'XS') {
    accumulator[label] = (...args) => css`
      @media (max-width: ${(props) => getBreakpoints(props).XS}px) {
        ${css(...args)}
      }
    `;
  } else {
    accumulator[label] = (...args) => css`
      @media (min-width: ${(props) => getBreakpoints(props)[label]}px) {
        ${css(...args)}
      }
    `;
  }
  return accumulator;
}, {});

export default media;
