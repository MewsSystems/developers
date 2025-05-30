import {IMAGE_BASE_URL} from '../../../api/constants';
import {
  MovieCoverImage,
  MovieCoverContainer,
  PlaceholderContainer,
  PlaceholderIcon,
} from './MovieCover.styled';

type MovieCoverProps = {
  poster_path: string | null;
  title: string;
  isMinimized?: boolean;
};

export default function MovieCover({poster_path, title, isMinimized = false}: MovieCoverProps) {
  return (
    <MovieCoverContainer $isMinimized={isMinimized}>
      {poster_path ? (
        <MovieCoverImage src={`${IMAGE_BASE_URL}${poster_path}`} alt={`${title} poster`} />
      ) : (
        <PlaceholderContainer>
          <PlaceholderIcon />
        </PlaceholderContainer>
      )}
    </MovieCoverContainer>
  );
}
