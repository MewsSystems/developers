import MovieDetail from '../components/MovieDetail/MovieDetail';
import { useMovies } from '../hooks/useMovies';

const MovieDetailView = () => {
	const { movie } = useMovies();

	return <>{movie && <MovieDetail movie={movie} />}</>;
};

export default MovieDetailView;
