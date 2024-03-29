import React from 'react';
import { useLocation } from 'react-router-dom';
import { useInjection } from "inversify-react";
import { MovieStore } from "./movie.store";
import { observer } from "mobx-react";

export const MoviePage = observer(() => {
    const movieStore = useInjection(MovieStore);
    const location = useLocation();
    movieStore.setLocationState(location.state);
    const movie = movieStore.movieInfo;

    return (
        <div className="movie-info">
            <img
                className="movie-info__poster"
                src={`https://image.tmdb.org/t/p/w200${movie?.posterPath}`}
                alt={movie?.title}
            />
            <div className="movie-info__title">{movie?.title}</div>
            <div className="movie-info__overview">{movie?.overview}</div>
            {/*<div className="movie-info__release-date">{movie?.releaseDate?.toDateString()}</div>*/}
            <div className="movie-info__vote-average">{movie?.voteAverage}</div>
            <div className="movie-info__vote-count">{movie?.voteCount}</div>
            <div className="movie-info__original-language">{movie?.originalLanguage}</div>
            <div className="movie-info__original-title">{movie?.originalTitle}</div>
        </div>
    );
});