import LoadingComponent from '../components/Loading/LoadingComponent';
import MovieDetail from '../components/MovieDetail/MovieDetail';
import { useMovies } from '../hooks/useMovies';

const MovieDetailView = () => {
	const { movie, loading } = useMovies();

	return <>{loading ? <LoadingComponent /> : movie && <MovieDetail movie={movie} />}</>;
};

export default MovieDetailView;
