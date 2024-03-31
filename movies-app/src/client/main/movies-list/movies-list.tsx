import React from 'react';
import { observer } from 'mobx-react';
import { useInjection } from 'inversify-react';
import { MoviesStore } from '../movies.store';
import { MovieInfo } from './movie';
import { MoviesWrapper, MovieItem } from './movies-list.styled';

export const MoviesList = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <MoviesWrapper>
            {
                moviesStore.movies.map(movie => {
                    return (
                        <MovieItem key={movie.id}>
                            <MovieInfo movie={movie}/>
                        </MovieItem>
                    );
                })
            }
        </MoviesWrapper>
    );

});