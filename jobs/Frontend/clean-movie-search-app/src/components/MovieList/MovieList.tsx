import React from 'react';
import styled from 'styled-components';
import { Movie } from '../../api';
import { MovieCard } from '../MovieCard/MovieCard';

const Grid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 24px;
  padding: 24px;
`;

interface MovieListProps {
  movies: Movie[];
  onMovieClick: (id: number) => void;
}

export const MovieList: React.FC<MovieListProps> = ({
  movies,
  onMovieClick,
}) => (
  <Grid>
    {movies.map((movie) => (
      <MovieCard key={movie.id} movie={movie} onClick={onMovieClick} />
    ))}
  </Grid>
);
