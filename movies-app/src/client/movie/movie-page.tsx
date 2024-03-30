import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useInjection } from "inversify-react";
import { observer } from "mobx-react";
import { MovieStore } from "./movie.store";
import posterNotAvailable from '../common/images/not_available.jpg'
import { MovieDetails } from "./movie-info/movie-details";
import './movie-page.scss';

export const MoviePage = observer(() => {
    const movieStore = useInjection(MovieStore);
    const location = useLocation();
    const navigate = useNavigate();
    const goToHomePage = () => navigate(`/`);
    movieStore.setLocationState(location.state);
    const movie = movieStore.movieInfo;


    if (movieStore.couldNotLoadMovie) {
        return (
            <div className="movie-page">
                <div className="movie-page__error">Oops, something went wrong.</div>
                <div className="movie-page__home-button" onClick={goToHomePage}>Go to home page</div>
            </div>
        );
    }

    if (!movie) {
        return null;
    }

    return (
        <div className="movie-page">
            <div className="movie-page__title">{movie.title}</div>
            <div className="movie-page__info">
                <div className="movie-page__image-block">
                    <img
                        className="movie-page__poster"
                        src={movie.posterPath
                            ? `https://image.tmdb.org/t/p/w200${movie.posterPath}`
                            : posterNotAvailable}
                        alt={movie.title}
                    />
                </div>
                <div className="movie-page__details">
                    <MovieDetails movie={movie}/>
                </div>
            </div>
            <div className="movie-page__overview">{movie.overview}</div>
        </div>
    );
});