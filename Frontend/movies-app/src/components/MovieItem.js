import React from 'react';
import { useHistory } from 'react-router-dom';
import { SubTitle, MovieCard } from './styled';

const MovieItem = ({ movie }) => {
  const history = useHistory();

  return (
    <MovieCard onClick={() => history.push(`/${movie.id}`)}>
      <SubTitle>{movie.title}</SubTitle>
      <img
        src={`https://image.tmdb.org/t/p/w154${movie.poster_path}`}
        alt={movie.title}
        onError={e => {
          e.target.onerror = null;
          e.target.src = 'no_image.png';
        }}
      />
    </MovieCard>
  );
};

export default MovieItem;
