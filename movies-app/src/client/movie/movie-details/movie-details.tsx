import React from 'react';
import {observer} from "mobx-react";
import { Movie } from "~data/types";
import './movie-details.scss';

type MovieProps = Readonly<{ movie: Movie }>;
export const MovieInfo = observer(({ movie }: MovieProps) => {
    return (
        <div className="movie-details">
            <div className="movie-details__title">Original title</div>
            <div className="movie-details__value">{movie.originalTitle}</div>
            <div className="movie-details__title">Original language</div>
            <div className="movie-details__value">{movie.originalLanguage}</div>
            <div className="movie-details__title">Original language</div>
            <div className="movie-details__value">{movie.voteAverage}</div>
            {/*<div className="movie-page__vote-count">{movie.voteCount}</div>*/}
            <div className="movie-details__title">Release date</div>
            <div className="movie-details__value">{movie.releaseDate.toDateString()}</div>
        </div>
    );

});