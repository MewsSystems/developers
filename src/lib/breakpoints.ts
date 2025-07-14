export const BREAKPOINTS = [
  { key: '2xl', media: '(min-width: 1536px)' },
  { key: 'xl', media: '(min-width: 1280px)' },
  { key: 'lg', media: '(min-width: 1024px)' },
  { key: 'md', media: '(min-width: 768px)' },
  { key: 'sm', media: '(min-width: 640px)' },
  { key: 'default', media: undefined },
] as const;

export type BreakpointKey = (typeof BREAKPOINTS)[number]['key'];
