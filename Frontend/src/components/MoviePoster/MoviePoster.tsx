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
  const getUrls = usePosterUrls(posterPath);
  const [showPlaceholder, setShowPlaceholder] = useState(!posterPath);
  const [imgSrc] = getUrls();
  const srcSet = getUrls(width).join(', ');

  return (
    <Placeholder width={`${width}px`} height={height ? `${height}px` : 'auto'}>
      <img
        loading="lazy"
        alt={alt}
        src={imgSrc}
        srcSet={srcSet}
        style={showPlaceholder ? { display: 'none' } : {}}
        onLoad={() => setShowPlaceholder(false)}
        onError={() => setShowPlaceholder(true)}
        {...props}
      />
    </Placeholder>
  );
}

export default MoviePoster;
