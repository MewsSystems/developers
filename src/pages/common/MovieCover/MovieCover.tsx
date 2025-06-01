import {
  MovieCoverImage,
  MovieCoverContainer,
  PlaceholderContainer,
  PlaceholderIcon,
} from './styled';
import {getImageUrl} from '../../../utils/getImageUrl';

type MovieCoverProps = {
  poster_path: string | null;
  title: string;
  isMinimized?: boolean;
};

export default function MovieCover({poster_path, title, isMinimized = false}: MovieCoverProps) {
  const imageUrl = getImageUrl(poster_path);

  return (
    <MovieCoverContainer $isMinimized={isMinimized}>
      {poster_path ? (
        <MovieCoverImage src={imageUrl!} alt={`${title} poster`} />
      ) : (
        <PlaceholderContainer>
          <PlaceholderIcon />
        </PlaceholderContainer>
      )}
    </MovieCoverContainer>
  );
}

MovieCover.displayName = 'MovieCover';
