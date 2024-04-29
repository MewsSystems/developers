import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useDebounce } from '@uidotdev/usehooks';
import Grid from '@mui/material/Grid';
import Divider from '@mui/material/Divider/Divider';
import { MovieListContainerProps } from './movies-list-container.interface';
import { Movie, MovieList } from '../../../shared/interfaces/movie.interface';
import Card from '../../../shared/components/card/card';
import FeedbackContainer from '../feedback-container/feedback-container';
import CircularProgress from '@mui/material/CircularProgress';

const MovieListContainer = ({
  searchTerm,
  page,
  total,
  onFetchMoviesSuccess,
  onFetchMoviesError,
}: MovieListContainerProps) => {
  const [movies, setMovies] = useState<Movie[] | null>(null);
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();
  const debouncedSearchTerm = useDebounce(searchTerm, 300);

  useEffect(() => {
    const updateURL = () => {
      const params = new URLSearchParams(location.search);
      const currentSearchTerm = params.get('search');
      if (currentSearchTerm !== searchTerm) {
        params.set('search', searchTerm ? searchTerm : '');
        navigate(`?${params.toString()}`);
      }
    };

    const fetchMovies = async () => {
      if (debouncedSearchTerm) {
        setLoading(true);
        try {
          const response = await fetch(
            `https://api.themoviedb.org/3/search/movie?page=${page}&query=${encodeURIComponent(
              debouncedSearchTerm ? debouncedSearchTerm : '',
            )}&include_adult=false&language=en-US&&api_key=${process.env.THEMOVIEDB_API_KEY}`,
          );
          if (!response.ok) {
            onFetchMoviesError();
            return;
          }
          const result: MovieList = await response.json();
          setMovies(result.results);
          onFetchMoviesSuccess(result);
        } catch (error) {
          onFetchMoviesError();
        } finally {
          setLoading(false);
        }
      }

      updateURL();
    };

    fetchMovies();
  }, [page, debouncedSearchTerm]);

  return (
    <Grid item xs={12}>
      {loading && (
        <Grid
          item
          className="d-flex d-justify-content-center d-align-items-center p-t-16"
        >
          <CircularProgress size={60} />
        </Grid>
      )}
      {!loading && searchTerm && movies && movies.length > 0 && (
        <>
          <FeedbackContainer title={`Total ${total} results`} />
          {movies.map((movie: Movie) => (
            <Grid
              key={movie.id}
              container
              alignItems="center"
              className="p-t-16"
            >
              <Grid item xs={1} lg={3} />
              <Grid item xs={10} lg={6}>
                <Link
                  to={`/movie/${movie.id}`}
                  style={{ textDecoration: 'none', color: 'inherit' }}
                >
                  <Card
                    imageUrl={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                    title={movie.title}
                    subtitle={movie.release_date?.split('-')?.[0]}
                    description={movie.overview}
                  />
                </Link>
                <Divider style={{ width: '100%' }} className="p-t-16" />
              </Grid>
            </Grid>
          ))}
        </>
      )}
      {!loading && searchTerm && movies && movies.length === 0 && (
        <FeedbackContainer
          title="No results"
          subtitle="No matches results for your search"
        />
      )}
    </Grid>
  );
};

export default MovieListContainer;
