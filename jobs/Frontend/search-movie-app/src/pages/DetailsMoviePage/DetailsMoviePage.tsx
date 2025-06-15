import { useParams } from 'react-router';
import { useGetDetailsMovie } from '../../hooks';

const DetailsMoviePage = () => {
  const { movieId } = useParams();
  const { data: detailsMovie, isLoading } = useGetDetailsMovie(movieId || '');

  return detailsMovie && !isLoading ? (
    <div>
      <p>Title: {detailsMovie.title}</p>
      <p>Overview: {detailsMovie.overview}</p>
      <p>Release date: {detailsMovie.release_date}</p>
    </div>
  ) : null;
};

export { DetailsMoviePage };
