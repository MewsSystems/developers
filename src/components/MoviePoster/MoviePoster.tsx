import React from 'react';
import { AiOutlineFileImage } from 'react-icons/ai';
import type { PosterUrl } from '@/types/api';

interface MoviePosterProps {
  posterUrl: PosterUrl;
  alt: string;
}

export function MoviePoster({ posterUrl, alt }: MoviePosterProps) {
  return (
    <div
      className={`
        flex justify-center rounded-md
        ${!posterUrl.default ? 'bg-stone-100 items-center' : 'items-start'}
        ${!posterUrl.sm ? 'sm:bg-stone-100 sm:items-center' : 'sm:items-start'}
        ${!posterUrl.md ? 'md:bg-stone-100 md:items-center' : 'md:items-start'}
        w-[92px] h-[138px]
        sm:w-[154px] sm:h-[231px]
        md:w-[185px] md:h-[278px]
      `}
    >
      <AiOutlineFileImage
        className={`
          ${posterUrl.default ? 'hidden' : 'block'}
          ${posterUrl.sm ? 'sm:hidden' : 'sm:block'}
          ${posterUrl.md ? 'md:hidden' : 'md:block'}
          text-stone-400 text-4xl
        `}
        aria-hidden
      />

      {(posterUrl.default || posterUrl.sm || posterUrl.md) && (
        <picture
          className={`
            ${posterUrl.default ? 'block' : 'hidden'}
            ${posterUrl.sm ? 'sm:block' : 'sm:hidden'}
            ${posterUrl.md ? 'md:block' : 'md:hidden'}
            w-full h-full
          `}
        >
          {posterUrl.md && <source srcSet={posterUrl.md} media="(min-width: 768px)" />}
          {posterUrl.sm && <source srcSet={posterUrl.sm} media="(min-width: 640px)" />}
          <img
            src={posterUrl.default ?? ''}
            alt={alt}
            width={92}
            height={138}
            className="border border-cyan-500 rounded-md object-contain w-full h-auto max-w-full"
            loading="lazy"
            decoding="async"
          />
        </picture>
      )}
    </div>
  );
}
