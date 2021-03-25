import { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { LoadingState, ErrorState } from '../EmptyState';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { fetchMovieDetails } from '../../redux/movieReducer';
import { MovieParams } from '../../services/tmdbApi';
import Movie from './Movie';

function MoviePage() {
  const dispatch = useAppDispatch();
  const { movieId } = useParams<MovieParams>();
  const { isLoading, error, timestamp, ...movie } = useAppSelector(
    (state) => state.movie
  );

  useEffect(() => {
    dispatch(fetchMovieDetails({ movieId }));
  }, [dispatch, movieId]);

  if (error) {
    return (
      <ErrorState title={error.name}>
        <p>{error.message}</p>
      </ErrorState>
    );
  }

  if (isLoading || parseInt(movieId) !== movie.id) {
    return <LoadingState title="Loading movie..." />;
  }

  return <Movie movie={movie} />;
}

export default MoviePage;
