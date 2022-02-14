import {Grid} from '@mui/material';
import React from 'react';
import {selectAllSearchedMovies} from './moviesSlice';
import {useAppSelector} from '../../app/hooks';
import {MoviesListItem} from './MoviesListItem';

export function MoviesList() {
  const movies = useAppSelector(selectAllSearchedMovies);

  return (
    <Grid container spacing={2} justifyContent="center" alignItems="stretch">
      {movies.map((movie) => (
        <Grid item key={movie.id}>
          <MoviesListItem item={movie} />
        </Grid>
      ))}
    </Grid>
  );
}
