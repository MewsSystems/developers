import { useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import EmptyState from '../components/EmptyState';
import ErrorState from '../components/EmptyState/ErrorState';
import Layout from '../components/Layout';
import { useAppDispatch, useAppSelector } from '../hooks';
import { fetchMovieDetails } from '../redux/movieReducer';
import { MovieParams } from '../services/tmdbApi';

function MovieDetail() {
  const dispatch = useAppDispatch();
  const { movieId } = useParams<MovieParams>();
  const { isLoading, error, timestamp, ...movie } = useAppSelector(
    (state) => state.movie
  );

  useEffect(() => {
    dispatch(fetchMovieDetails({ movieId }));
  }, [dispatch, movieId]);

  useEffect(() => {
    document.title = [movie.title, 'Movie Search'].join(' - ');
  }, [movie]);

  if (isLoading) {
    return <EmptyState title="Loading movie..." />;
  }

  return (
    <Layout header={movie.title}>
      {error ? (
        <ErrorState title="Movie Not Found">
          <Link to="/">Go back to Search</Link>
        </ErrorState>
      ) : (
        <pre>{JSON.stringify(movie, null, 2)}</pre>
      )}
    </Layout>
  );
}

export default MovieDetail;
