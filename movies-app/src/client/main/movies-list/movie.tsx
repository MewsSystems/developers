import React from 'react';
import { observer } from 'mobx-react';
import { useNavigate } from 'react-router-dom';
import { Movie } from '~data/types';
import { IMAGE_BASE_URL } from '~data/api/constants';
import posterNotAvailable from '../../common/images/not_available.jpg'
import { MovieWrapper, MoviePoster, MovieTitle } from './movie.styled'

type MovieProps = Readonly<{ movie: Movie }>;
export const MovieInfo = observer(({movie}: MovieProps) => {
    const navigate = useNavigate();
    const goToMovie = () => {
        navigate(`/movie/${movie.id}`, {state: {movie: JSON.stringify(movie)}});
    }

    return (
        <MovieWrapper onClick={goToMovie}>
            <MovieTitle>{movie.title}</MovieTitle>
            <MoviePoster
                src={movie.posterPath ? `${IMAGE_BASE_URL}${movie.posterPath}` : posterNotAvailable}
                alt={movie.title}
            />
        </MovieWrapper>
    );

});