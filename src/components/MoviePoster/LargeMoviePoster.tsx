import React from 'react';
import { AiOutlineFileImage } from 'react-icons/ai';
import type { DetailedPosterUrl } from '@/types/api';

interface LargeMoviePosterProps {
  posterUrl: DetailedPosterUrl;
  alt: string;
}

export function LargeMoviePoster({ posterUrl, alt }: LargeMoviePosterProps) {
  return (
    <div
      className={`
        flex justify-center rounded-md
        ${!posterUrl.default ? 'bg-stone-100 items-center' : 'items-start'}
        ${!posterUrl.sm ? 'sm:bg-stone-100 sm:items-center' : 'sm:items-start'}
        ${!posterUrl.lg ? 'lg:bg-stone-100 lg:items-center' : 'lg:items-start'}
        w-[185px] h-[278px]
        sm:w-[342px] sm:h-[513px]
        lg:w-[500px] lg:h-[750px]
      `}
    >
      <AiOutlineFileImage
        className={`
          ${posterUrl.default ? 'hidden' : 'block'}
          ${posterUrl.sm ? 'sm:hidden' : 'sm:block'}
          ${posterUrl.lg ? 'lg:hidden' : 'lg:block'}
          text-stone-400 text-4xl
        `}
        aria-hidden
      />

      {(posterUrl.default || posterUrl.sm || posterUrl.lg) && (
        <picture
          className={`
            ${posterUrl.default ? 'block' : 'hidden'}
            ${posterUrl.sm ? 'sm:block' : 'sm:hidden'}
            ${posterUrl.lg ? 'lg:block' : 'lg:hidden'}
            w-full h-full
          `}
        >
          {posterUrl.lg && <source srcSet={posterUrl.lg} media="(min-width: 1024px)" />}
          {posterUrl.sm && <source srcSet={posterUrl.sm} media="(min-width: 640px)" />}
          <img
            src={posterUrl.default || ''}
            alt={alt}
            width={185}
            height={278}
            className="border border-cyan-500 rounded-md object-contain w-full h-auto max-w-full"
            loading="lazy"
            decoding="async"
            fetchPriority="high"
          />
        </picture>
      )}
    </div>
  );
}
