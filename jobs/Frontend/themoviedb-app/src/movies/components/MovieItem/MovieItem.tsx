import { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from '../../../shared/enums/AppRoutes';
import { Movie } from '../../types/MovieSearchTypes';
import StyledMovieItem, {
    StyledFallbackPoster,
    StyledMovieItemButton,
    StyledPhotoIcon,
    StyledPoster,
} from './MovieItem.styles';

const MovieItem: FC<Movie> = (movie) => {
    const { id, title, poster_path } = movie;
    const navigate = useNavigate();
    return (
        <StyledMovieItem>
            {poster_path ? (
                <StyledPoster
                    src={`https://image.tmdb.org/t/p/w500/${poster_path}`}
                    alt="Movie poster"
                />
            ) : (
                <StyledFallbackPoster>
                    <StyledPhotoIcon /> <p>No poster.</p>
                </StyledFallbackPoster>
            )}
            <h4>{title}</h4>
            <StyledMovieItemButton
                onClick={() =>
                    navigate(
                        AppRoutes.MovieDetails.replace(':movieId', String(id))
                    )
                }>
                View details
            </StyledMovieItemButton>
        </StyledMovieItem>
    );
};

export default MovieItem;
