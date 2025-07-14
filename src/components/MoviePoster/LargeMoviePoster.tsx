import type { DetailedPosterUrl } from '@/types/api';
import { type BreakpointDefinition, ResponsiveImage } from '@/components/ResponsiveImage';

interface LargeMoviePosterProps {
  posterUrl: DetailedPosterUrl;
  alt: string;
}

export function LargeMoviePoster({ posterUrl, alt }: LargeMoviePosterProps) {
  const breakpointDefinition: BreakpointDefinition = {
    default: {
      src: posterUrl.default,
      containerSize: 'w-[185px] h-[278px]',
    },
    sm: {
      src: posterUrl.sm,
      containerSize: 'sm:w-[342px] sm:h-[513px]',
    },
    lg: {
      src: posterUrl.lg,
      containerSize: 'lg:w-[500px] lg:h-[750px]',
    },
  };

  return (
    <ResponsiveImage
      breakpointDefinition={breakpointDefinition}
      alt={alt}
      width={185}
      height={278}
      fetchPriority="high"
    />
  );
}
