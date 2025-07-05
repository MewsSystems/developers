import type { ReactElement } from 'react';
import { AiOutlineFileImage } from 'react-icons/ai';

export const BREAKPOINTS = [
  { key: '2xl', media: '(min-width: 1536px)' },
  { key: 'xl', media: '(min-width: 1280px)' },
  { key: 'lg', media: '(min-width: 1024px)' },
  { key: 'md', media: '(min-width: 768px)' },
  { key: 'sm', media: '(min-width: 640px)' },
  { key: 'default', media: undefined },
] as const;

export type BreakpointKey = (typeof BREAKPOINTS)[number]['key'];

export interface BreakpointValue {
  src: string | null;
  containerSize: string;
}

export type ContainerStyle = {
  withSrc: string;
  withoutSrc: string;
};

export type VisibilityStyle = {
  visible: string;
  hidden: string;
};

export const containerStyles: Record<BreakpointKey, ContainerStyle> = {
  default: {
    withoutSrc: 'bg-stone-100 items-center',
    withSrc: 'items-start',
  },
  sm: {
    withoutSrc: 'sm:bg-stone-100 sm:items-center',
    withSrc: 'sm:items-start',
  },
  md: {
    withoutSrc: 'md:bg-stone-100 md:items-center',
    withSrc: 'md:items-start',
  },
  lg: {
    withoutSrc: 'lg:bg-stone-100 lg:items-center',
    withSrc: 'lg:items-start',
  },
  xl: {
    withoutSrc: 'xl:bg-stone-100 xl:items-center',
    withSrc: 'xl:items-start',
  },
  '2xl': {
    withoutSrc: '2xl:bg-stone-100 2xl:items-center',
    withSrc: '2xl:items-start',
  },
};

export const visibilityStyles: Record<BreakpointKey, VisibilityStyle> = {
  default: {
    visible: 'block',
    hidden: 'hidden',
  },
  sm: {
    visible: 'sm:block',
    hidden: 'sm:hidden',
  },
  md: {
    visible: 'md:block',
    hidden: 'md:hidden',
  },
  lg: {
    visible: 'lg:block',
    hidden: 'lg:hidden',
  },
  xl: {
    visible: 'xl:block',
    hidden: 'xl:hidden',
  },
  '2xl': {
    visible: '2xl:block',
    hidden: '2xl:hidden',
  },
};

export type BreakpointDefinition = {
  default: BreakpointValue;
} & {
  [K in Exclude<BreakpointKey, 'default'>]?: BreakpointValue;
};

export type FetchPriority = 'auto' | 'high' | 'low';

interface ResponsiveImageProps {
  breakpointDefinition: BreakpointDefinition;
  alt: string;
  width: number;
  height: number;
  fetchPriority?: FetchPriority;
}

export function ResponsiveImage({
  breakpointDefinition,
  alt,
  fetchPriority = 'auto',
  width,
  height,
}: ResponsiveImageProps) {
  const BREAKPOINT_KEYS = BREAKPOINTS.map((breakpoint) => breakpoint.key);

  const sortedBreakpoints = Object.fromEntries(
    Object.entries(breakpointDefinition).sort(
      ([aKey], [bKey]) =>
        BREAKPOINT_KEYS.indexOf(aKey as (typeof BREAKPOINT_KEYS)[number]) -
        BREAKPOINT_KEYS.indexOf(bKey as (typeof BREAKPOINT_KEYS)[number])
    )
  );

  return (
    <div
      className={`
        flex justify-center rounded-md
        ${Object.entries(sortedBreakpoints)
          .map(
            ([breakpointKey, breakpointValues]) =>
              `${breakpointValues.containerSize} ${
                breakpointValues.src
                  ? containerStyles[breakpointKey as BreakpointKey].withSrc
                  : containerStyles[breakpointKey as BreakpointKey].withoutSrc
              }`
          )
          .join(' ')}
      `}
    >
      <AiOutlineFileImage
        className={`
          ${Object.entries(sortedBreakpoints)
            .map(([breakpointKey, breakpointValues]) =>
              breakpointValues.src
                ? visibilityStyles[breakpointKey as BreakpointKey].hidden
                : visibilityStyles[breakpointKey as BreakpointKey].visible
            )
            .join(' ')}
          text-stone-400 text-4xl
        `}
        aria-hidden
      />

      {Object.values(sortedBreakpoints).some((breakpoint) => !!breakpoint.src) && (
        <picture
          className={`
          ${Object.entries(sortedBreakpoints)
            .map(([breakpointKey, breakpointValues]) =>
              breakpointValues.src
                ? visibilityStyles[breakpointKey as BreakpointKey].visible
                : visibilityStyles[breakpointKey as BreakpointKey].hidden
            )
            .join(' ')}
            w-full h-full
          `}
        >
          {Object.entries(sortedBreakpoints).reduce<ReactElement[]>(
            (accum, [breakpointKey, breakpointValues]) => {
              if (breakpointValues.src && breakpointKey !== 'default') {
                const mediaQuery = BREAKPOINTS.find(
                  (breakpoint) => breakpoint.key === breakpointKey
                )?.media;

                const key = `${breakpointKey}:${breakpointValues.src}`;

                accum.push(<source key={key} srcSet={breakpointValues.src} media={mediaQuery} />);
              }

              return accum;
            },
            []
          )}
          <img
            {...(sortedBreakpoints.default.src ? { src: sortedBreakpoints.default.src } : {})}
            alt={alt}
            width={width}
            height={height}
            className="border border-stone-400 rounded-md object-contain w-full h-auto max-w-full"
            loading="lazy"
            decoding="async"
            fetchPriority={fetchPriority}
          />
        </picture>
      )}
    </div>
  );
}
