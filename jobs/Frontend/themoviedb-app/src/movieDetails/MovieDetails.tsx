import { Link, useParams } from 'react-router-dom';
import AppRoutes from '../shared/enums/AppRoutes';
import LocalStoreUtil from '../shared/utils/LocalStorageUtil';
import { useGetMovieDetailsQuery } from './api/movieDetailsApiSlice';

const MovieDetails = () => {
    const { movieId } = useParams();

    const {
        data: movie,
        isLoading,
        isFetching,
    } = useGetMovieDetailsQuery({
        movieId: movieId || '',
    });

    if (isLoading || isFetching) {
        return <p>Loading...</p>;
    }

    if (!movie) {
        return <p>No movie found.</p>;
    }

    const { poster_path, title, overview } = movie;

    return (
        <div>
            <Link
                to={AppRoutes.Movies}
                onClick={() =>
                    LocalStoreUtil.set('fromMovieDetailsPage', true)
                }>
                Go back
            </Link>
            <img src={`https://image.tmdb.org/t/p/w154/${poster_path}`} />
            <h2>{title}</h2>
            <p>{overview}</p>
        </div>
    );
};

export default MovieDetails;
