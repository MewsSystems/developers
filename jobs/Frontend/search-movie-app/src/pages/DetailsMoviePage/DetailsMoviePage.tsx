import { useParams } from 'react-router';
import { CardDetailsMovie, CardDetailsMovieSkeleton } from './components';
import { useGetDetailsMovie } from '../../hooks';
import { detailsMovieAdapter } from '../../adapters/detailsMovieAdapter';

export const DetailsMoviePage = () => {
  const { movieId } = useParams();
  const { data, isLoading } = useGetDetailsMovie(movieId || '');

  const detailsMovie = data && detailsMovieAdapter(data);

  return detailsMovie && !isLoading ? (
    <CardDetailsMovie data={detailsMovie} />
  ) : (
    <CardDetailsMovieSkeleton />
  );
};
