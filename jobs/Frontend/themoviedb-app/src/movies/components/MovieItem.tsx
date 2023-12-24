import { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from '../../shared/enums/AppRoutes';
import { Movie } from '../models/MovieSearchModels';

const MovieItem: FC<Movie> = (movie) => {
    const { id, original_title, overview, poster_path } = movie;
    const navigate = useNavigate();
    return (
        <li
            onClick={() =>
                navigate(AppRoutes.MovieDetails.replace(':movieId', String(id)))
            }>
            <img src={`https://image.tmdb.org/t/p/w154/${poster_path}`} />
            <h3>{original_title}</h3>
            <p>{overview}</p>
        </li>
    );
};

export default MovieItem;
