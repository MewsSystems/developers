import React from 'react';

import { Row } from './styled';
import MovieItem from './MovieItem';

const MovieList = ({ movies, empty = null }) => {
  return <Row>{movies && movies.length ? movies.map(movie => <MovieItem movie={movie} key={movie.id} />) : empty}</Row>;
};

export default MovieList;
