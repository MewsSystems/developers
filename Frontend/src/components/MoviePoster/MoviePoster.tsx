import { useState } from 'react';
import { useImageConfig } from '../../hooks';
import { MoviePosterSkeleton } from './styled';

interface MoviePosterProps extends React.HTMLAttributes<HTMLImageElement> {
  posterPath: string;
  alt: HTMLImageElement['alt'];
  width: string;
  height?: string;
}

const MoviePoster = ({
  posterPath,
  alt,
  width,
  height,
  ...props
}: MoviePosterProps) => {
  const { poster_sizes, getURLs } = useImageConfig();
  const posterURLs = getURLs(posterPath, poster_sizes);
  const srcSet = posterURLs
    .map((url, index) => `${url} ${index + 1}x`)
    .join(', ');
  const hasPosterPath = posterPath !== null;
  const [showSkeleton, setShowSkeleton] = useState(!hasPosterPath);

  return (
    <MoviePosterSkeleton width={width} height={height}>
      <img
        loading="lazy"
        alt={alt}
        src={posterURLs[0]}
        srcSet={srcSet}
        style={showSkeleton ? { display: 'none' } : {}}
        onLoad={() => setShowSkeleton(false)}
        onError={() => setShowSkeleton(true)}
        {...props}
      />
    </MoviePosterSkeleton>
  );
};

export default MoviePoster;
