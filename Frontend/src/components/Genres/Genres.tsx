import { ReactElement } from 'react';
import { MovieDetail } from '../../services/tmdbApi';
import { GenreBox, GenreContainer } from './styled';

interface GenresProps {
  genres: MovieDetail['genres'];
}

function Genres({ genres }: GenresProps): ReactElement {
  return (
    <GenreContainer gap="0.5em">
      {genres.map((genre) => (
        <GenreBox key={genre.id}>{genre.name}</GenreBox>
      ))}
    </GenreContainer>
  );
}

export default Genres;
