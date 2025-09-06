type PosterImageProps = {
  posterPath?: string | null;
  alt: string;
  maxWidth?: string;
};

const PosterImage = ({ posterPath, alt, maxWidth }: PosterImageProps) => {
  return (
    <img
      src={`https://image.tmdb.org/t/p/w500${posterPath}`}
      alt={alt}
      style={{ maxWidth }}
    />
  );
};

export default PosterImage;
