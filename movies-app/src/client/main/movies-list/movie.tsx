import React from 'react';
import {observer} from "mobx-react";
import { useNavigate } from "react-router-dom";
import { Movie } from "~data/types";
import { IMAGE_BASE_URL } from "~data/api/constants";
import posterNotAvailable from '../../common/images/not_available.jpg'
import './movie.scss';

type MovieProps = Readonly<{ movie: Movie }>;
export const MovieInfo = observer(({ movie }: MovieProps) => {
    const navigate = useNavigate();
    const goToMovie = () => {
        navigate(`/movie/${movie.id}`, { state: { movie: JSON.stringify(movie) }});
    }

    return (
        <div className="movie" onClick={goToMovie}>
            <div className="movie__title">{movie.title}</div>
            <img
                className="movie__poster"
                src={movie.posterPath ? `${IMAGE_BASE_URL}${movie.posterPath}` : posterNotAvailable}
                alt={movie.title}
            />
        </div>
    );

});