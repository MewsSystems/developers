import React from 'react';
import {observer} from "mobx-react";
import {resolve, useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "./movies.store";
import { Movie } from "../../data/types";

type MovieProps = Readonly<{ movie: Movie }>;
export const MovieInfo = observer(({ movie }: MovieProps) => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <div className="movie">
            Movie
        </div>
    );

});