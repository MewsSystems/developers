import {
  PlaceholderIcon,
  PosterContainer,
  PosterImage,
  PosterPlaceholder,
} from '../MovieCard.styled';
import {IMAGE_BASE_URL} from '../../../../../api/constants';

type MovieCoverProps = {poster_path: string | null; title: string};

export default function MovieCover({poster_path, title}: MovieCoverProps) {
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
}
