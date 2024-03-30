import React from 'react';
import { observer } from "mobx-react";
import { useInjection } from "inversify-react";
import { MoviesStore } from "../movies.store";
import { MovieInfo } from './movie';
import './movies-list.scss';

export const MoviesList = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <div className="movies-list">
            {
                moviesStore.movies.map(movie => {
                    return (
                        <div key={movie.id} className="movies-list__movie">
                            <MovieInfo movie={movie}/>
                        </div>
                    );
                })
            }
        </div>
    );

});