import LoadingComponent from '../components/Loading/LoadingComponent';
import MovieDetail from '../components/MovieDetail/MovieDetail';
import { useMovies } from '../hooks/useMovies';

const MovieDetailView = () => {
	const { movie, loading } = useMovies();

	return (
		<div className='movie-detail-view'>
			{loading ? <LoadingComponent /> : movie && <MovieDetail movie={movie} />}
		</div>
	);
};

export default MovieDetailView;
