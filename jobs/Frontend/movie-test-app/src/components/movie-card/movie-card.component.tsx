import { FC } from 'react';
import { Result } from '../../types/api.ts';
import { Footer, MovieContainer, Name } from './movie-card.styles.tsx';
import { Link } from 'react-router-dom';

type ProductCardProps = {
  movie: Result;
};

const MovieCard: FC<ProductCardProps> = ({ movie }) => {
  const { title } = movie;

  return (
    <MovieContainer>
      <Footer>
        <Link to={`/movies/${movie?.id}`}>{movie?.title}</Link>
        <Name>{title}</Name>
      </Footer>
    </MovieContainer>
  );
};

export { MovieCard };
