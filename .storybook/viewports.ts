import { MINIMAL_VIEWPORTS } from 'storybook/viewport';

export const viewports = {
  ...MINIMAL_VIEWPORTS,
  xs: {
    name: 'xs (360px)',
    styles: { width: '360px', height: '100%' },
    type: 'mobile',
  },
  xsLargest: {
    name: 'xs (639px)',
    styles: { width: '639px', height: '100%' },
    type: 'mobile',
  },
  sm: {
    name: 'sm (640px)',
    styles: { width: '640px', height: '100%' },
    type: 'mobile',
  },
  smLargest: {
    name: 'sm (767px)',
    styles: { width: '767px', height: '100%' },
    type: 'mobile',
  },
  md: {
    name: 'md (768px)',
    styles: { width: '768px', height: '100%' },
    type: 'tablet',
  },
  mdLargest: {
    name: 'md (1023px)',
    styles: { width: '1023px', height: '100%' },
    type: 'tablet',
  },
  lg: {
    name: 'lg (1024px)',
    styles: { width: '1024px', height: '100%' },
    type: 'desktop',
  },
  xl: {
    name: 'xl (1280px)',
    styles: { width: '1280px', height: '100%' },
    type: 'desktop',
  },
  '2xl': {
    name: '2xl (1536px)',
    styles: { width: '1536px', height: '100%' },
    type: 'desktop',
  },
};
