import { useState } from 'react';
import { usePosterUrls } from '../../hooks';
import { Placeholder } from './styled';

interface MoviePosterProps extends React.HTMLAttributes<HTMLImageElement> {
  posterPath: string;
  alt: HTMLImageElement['alt'];
  width: number;
  height?: number;
}

function MoviePoster({
  posterPath,
  alt,
  width,
  height,
  ...props
}: MoviePosterProps) {
  const getUrls = usePosterUrls(posterPath, width);
  const [showSkeleton, setShowSkeleton] = useState(!posterPath);
  const [imgSrc] = getUrls();
  const srcSet = getUrls(true).join(', ');

  return (
    <Placeholder width={`${width}px`} height={height ? `${height}px` : 'auto'}>
      <img
        loading="lazy"
        alt={alt}
        src={imgSrc}
        srcSet={srcSet}
        style={showSkeleton ? { display: 'none' } : {}}
        onLoad={() => setShowSkeleton(false)}
        onError={() => setShowSkeleton(true)}
        {...props}
      />
    </Placeholder>
  );
}

export default MoviePoster;
