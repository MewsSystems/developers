import React from 'react';
import {observer} from "mobx-react";
import {resolve, useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "../movies.store";
import { Movie } from "../../../data/types";
import posterNotAvailable from './images/not_available.jpg'
import './movie.scss';

type MovieProps = Readonly<{ movie: Movie }>;
const IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w200';
export const MovieInfo = observer(({ movie }: MovieProps) => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <div className="movie">
            <div className="movie__title">{movie.title}</div>
            <img
                className="movie__poster"
                src={movie.poster_path ? `${IMAGE_BASE_URL}${movie.poster_path}` : posterNotAvailable}
                alt={movie.title}
            />
        </div>
    );

});