import React from 'react';
import { Movie } from '../types';
import MovieCard from './MovieCard';
import styled from 'styled-components';

const Grid = styled.div`
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
`;

interface MovieListProps {
  movies: Movie[];
}

const MovieList: React.FC<MovieListProps> = ({ movies }) => {
  return (
    <Grid>
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </Grid>
  );
};

export default MovieList;