import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useInjection } from 'inversify-react';
import { observer } from 'mobx-react';
import { MovieStore } from './movie.store';
import posterNotAvailable from '../common/images/not_available.jpg'
import { MovieDetails } from './movie-info/movie-details';
import {
    MoviePageWrapper,
    MovieOverview,
    MovieTitle,
    MovieDetailsWrapper,
    MovieInfo,
    MovieImageBlock,
    MoviePoster,
    HomeButton,
} from './movie-page.styled';

export const MoviePage = observer(() => {
    const movieStore = useInjection(MovieStore);
    const location = useLocation();
    const navigate = useNavigate();
    const goToHomePage = () => navigate(`/`);
    movieStore.setLocationState(location.state);
    const movie = movieStore.movieInfo;


    if (movieStore.couldNotLoadMovie) {
        return (
            <MoviePageWrapper>
                <div>Oops, something went wrong.</div>
                <HomeButton onClick={goToHomePage}>Go to home page</HomeButton>
            </MoviePageWrapper>
        );
    }

    if (!movie) {
        return null;
    }

    return (
        <MoviePageWrapper>
            <MovieTitle>{movie.title}</MovieTitle>
            <MovieInfo>
                <MovieImageBlock>
                    <MoviePoster
                        src={movie.posterPath
                            ? `https://image.tmdb.org/t/p/w200${movie.posterPath}`
                            : posterNotAvailable}
                        alt={movie.title}
                    />
                </MovieImageBlock>
                <MovieDetailsWrapper>
                    <MovieDetails movie={movie}/>
                </MovieDetailsWrapper>
            </MovieInfo>
            <MovieOverview>{movie.overview}</MovieOverview>
        </MoviePageWrapper>
    );
});