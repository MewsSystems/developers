import { css } from "styled-components";

export type DeviceSizes = keyof typeof device;

export const device = {
  xs: "320px",
  sm: "640px",
  md: "768px",
  lg: "1024px",
  xl: "1280px",
  xxl: "1536px",
};

export const media = {
  xs: (...args: Parameters<typeof css>) => css`
    @media (max-width: ${device.xs}) {
      ${css(...args)};
    }
  `,
  sm: (...args: Parameters<typeof css>) => css`
    @media (max-width: ${device.sm}) {
      ${css(...args)};
    }
  `,
  md: (...args: Parameters<typeof css>) => css`
    @media (max-width: ${device.md}) {
      ${css(...args)};
    }
  `,
  lg: (...args: Parameters<typeof css>) => css`
    @media (max-width: ${device.lg}) {
      ${css(...args)};
    }
  `,
  xl: (...args: Parameters<typeof css>) => css`
    @media (max-width: ${device.xl}) {
      ${css(...args)};
    }
  `,
  xxl: (...args: Parameters<typeof css>) => css`
    @media (max-width: ${device.xxl}) {
      ${css(...args)};
    }
  `,
};
