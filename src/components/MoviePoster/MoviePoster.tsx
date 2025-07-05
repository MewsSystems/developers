import type { PosterUrl } from '@/types/api';
import { type BreakpointDefinition, ResponsiveImage } from '@/components/ResponsiveImage';

interface MoviePosterProps {
  posterUrl: PosterUrl;
  alt: string;
}

export function MoviePoster({ posterUrl, alt }: MoviePosterProps) {
  const breakpointDefinition: BreakpointDefinition = {
    default: {
      src: posterUrl.default,
      containerSize: 'w-[92px] h-[138px]',
    },
    sm: {
      src: posterUrl.sm,
      containerSize: 'sm:w-[154px] sm:h-[231px]',
    },
    md: {
      src: posterUrl.md,
      containerSize: 'md:w-[185px] md:h-[278px]',
    },
  };

  return (
    <ResponsiveImage
      breakpointDefinition={breakpointDefinition}
      alt={alt}
      width={92}
      height={138}
    />
  );
}
