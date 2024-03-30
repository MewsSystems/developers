import React, { Fragment } from 'react';
import {observer} from "mobx-react";
import { Movie } from "~data/types";
import './movie-details.scss';

type MovieProps = Readonly<{ movie: Movie }>;
export const MovieDetails = observer(({ movie }: MovieProps) => {
    return (
        <div className="movie-details">
            {
                movie.originalTitle && (
                    <Fragment>
                        <div className="movie-details__title">Original title</div>
                        <div className="movie-details__value">{movie.originalTitle}</div>
                    </Fragment>
                )
            }
            {
                movie.originalLanguage && (
                    <Fragment>
                        <div className="movie-details__title">Original language</div>
                        <div className="movie-details__value">{movie.originalLanguage}</div>
                    </Fragment>
                )
            }
            {
                movie.voteAverage && (
                    <Fragment>
                        <div className="movie-details__title">Rating</div>
                        <div className="movie-details__value">{movie.voteAverage}</div>
                        {/*<div className="movie-page__vote-count">{movie.voteCount}</div>*/}
                    </Fragment>
                )
            }
            {
                movie.releaseDate && (
                    <Fragment>
                        <div className="movie-details__title">Release date</div>
                        <div className="movie-details__value">{movie.releaseDate.toDateString()}</div>
                    </Fragment>
                )
            }
        </div>
    );

});