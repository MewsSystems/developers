import React from 'react';
import {observer} from "mobx-react";
import {resolve, useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "./movies.store";
import { Movie } from "../../data/types";
import { MovieInfo } from './movie';

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