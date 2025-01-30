import { useSelector } from 'react-redux';
import MovieList from './MovieList';

const Favorites = () => {
  const favorites = useSelector((state: any) => state.movies.favorites);

  return (
    <div>
      <h2>Favorites</h2>
      <MovieList searchPerformed={true} movies={favorites} />
    </div>
  );
};

export default Favorites;
