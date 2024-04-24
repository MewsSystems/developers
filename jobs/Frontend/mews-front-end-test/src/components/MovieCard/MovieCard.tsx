import { FC } from 'react';
import { StyledCard } from './MovieCard.styled';
import { Movie } from '../../api/sendRequest';

interface Props {
  movie: Movie;
}

const MovieCard: FC<Props> = ({ movie }) => {
  return (
    <StyledCard>
      <p>{movie.title}</p>
    </StyledCard>
  );
};

export { MovieCard };
