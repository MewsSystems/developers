import React, { Fragment } from 'react';
import {observer} from "mobx-react";
import { Movie } from "~data/types";
import { MovieDetailsWrapper, InfoTitle } from "./movie-details.styled";

type MovieProps = Readonly<{ movie: Movie }>;
export const MovieDetails = observer(({ movie }: MovieProps) => {
    return (
        <MovieDetailsWrapper>
            {
                movie.originalTitle && (
                    <Fragment>
                        <InfoTitle>Original title</InfoTitle>
                        <div>{movie.originalTitle}</div>
                    </Fragment>
                )
            }
            {
                movie.originalLanguage && (
                    <Fragment>
                        <InfoTitle>Original language</InfoTitle>
                        <div>{movie.originalLanguage}</div>
                    </Fragment>
                )
            }
            {
                movie.voteAverage && (
                    <Fragment>
                        <InfoTitle>Rating</InfoTitle>
                        <div>{movie.voteAverage}</div>
                        {/*<div className="movie-page__vote-count">{movie.voteCount}</div>*/}
                    </Fragment>
                )
            }
            {
                movie.releaseDate && (
                    <Fragment>
                        <InfoTitle>Release date</InfoTitle>
                        <div>{movie.releaseDate.toDateString()}</div>
                    </Fragment>
                )
            }
        </MovieDetailsWrapper>
    );

});