import { useNavigate, useParams } from 'react-router-dom';
import Button from '../shared/components/Button/Button';
import AppRoutes from '../shared/enums/AppRoutes';
import LocalStoreUtil from '../shared/utils/LocalStorageUtil';
import { useGetMovieDetailsQuery } from './api/movieDetailsApiSlice';
import StyledMovieDetailsContainer, {
    StyledMovieDetailsContent,
} from './components/MovieDetailsContainer/MovieDetailsContainer.styles';
import MovieDetailsInfo from './components/MovieDetailsInfo/MovieDetailsInfo';

const MovieDetails = () => {
    const { movieId } = useParams();
    const navigate = useNavigate();

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

    const { poster_path } = movie;

    const handleGoBack = () => {
        navigate(AppRoutes.Movies);
        LocalStoreUtil.set('fromMovieDetailsPage', true);
    };

    return (
        <StyledMovieDetailsContainer>
            <Button variant="secondary" size="small" onClick={handleGoBack}>
                Go back
            </Button>

            <StyledMovieDetailsContent>
                <img src={`https://image.tmdb.org/t/p/w500/${poster_path}`} />

                <MovieDetailsInfo {...movie} />
            </StyledMovieDetailsContent>
        </StyledMovieDetailsContainer>
    );
};

export default MovieDetails;
