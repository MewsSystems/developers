import React, { useEffect, useState } from 'react';
import Grid from '@mui/material/Grid';
import Divider from '@mui/material/Divider/Divider';
import { MovieListContainerProps } from './movies-list-container.interface';
import { Movie, MovieList } from '../../../interfaces/movie.interface';
import Card from '../../../components/card/card';

const MovieListContainer = ({
  searchTerm,
  page,
  onFetchMoviesSuccess,
  onFetchMoviesError,
}: MovieListContainerProps) => {
  const [movies, setMovies] = useState<Movie[] | null>(null);

  useEffect(() => {
    const fetchMovies = async () => {
      if (searchTerm !== null) {
        try {
          const response = await fetch(
            `https://api.themoviedb.org/3/search/movie?page=${page}&query=${encodeURIComponent(
              searchTerm,
            )}&include_adult=false&language=en-US&&api_key=${process.env.THEMOVIEDB_API_KEY}`,
          );
          if (!response.ok) {
            onFetchMoviesError();
          }
          const result: MovieList = await response.json();
          setMovies(result.results);
          onFetchMoviesSuccess(result);
        } catch (error) {
          onFetchMoviesError();
        }
      }
    };

    fetchMovies();
  }, [searchTerm, page]);

  return (
    <Grid item xs={12}>
      {searchTerm &&
        movies &&
        movies.length > 0 &&
        movies.map((movie: Movie) => (
          <Grid key={movie.id} container alignItems="center" className="p-t-16">
            <Grid item xs={1} lg={3} />
            <Grid item xs={10} lg={6}>
              <Card
                imageUrl={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                title={movie.title}
                subtitle={movie.overview}
                releaseDate={movie.release_date?.split('-')?.[0]}
              />
            </Grid>
            <Grid item xs={1} lg={3} />
            <Grid item xs={1} lg={3} />
            <Grid item xs={10} lg={6}>
              <Divider style={{ width: '100%' }} className="p-t-16" />
            </Grid>
          </Grid>
        ))}
      {searchTerm && movies && movies.length === 0 && (
        <Grid container alignItems="center" className="p-t-16">
          No results
        </Grid>
      )}
    </Grid>
  );
};

export default MovieListContainer;
