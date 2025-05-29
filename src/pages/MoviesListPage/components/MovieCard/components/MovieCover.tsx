import {
  PlaceholderIcon,
  PosterContainer,
  PosterImage,
  PosterPlaceholder,
} from '../MovieCard.styled.tsx';

type MovieCoverProps = {poster_path: string | null; title: string};

export const MovieCover = ({poster_path, title}: MovieCoverProps) => {
  const IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w500';

  return (
    <PosterContainer>
      {poster_path ? (
        <PosterImage src={`${IMAGE_BASE_URL}${poster_path}`} alt={`${title} poster`} />
      ) : (
        <PosterPlaceholder>
          <PlaceholderIcon />
        </PosterPlaceholder>
      )}
    </PosterContainer>
  );
};
