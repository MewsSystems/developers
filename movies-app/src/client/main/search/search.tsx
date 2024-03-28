import React from 'react';
import {observer} from "mobx-react";
import {resolve, useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "../movies.store";
import './search.scss';


export const Search = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    const errorClass = classNames(
        'search__error',
        {
            'search__error_hidden': moviesStore.error === undefined,
        }
    );

    return (
        <div className="search">
            <input
                className="search__input"
                type="text"
                placeholder="Search for a movie"
                onChange={ev => moviesStore.searchForString(ev.target.value)}
            />
            <div className={errorClass}>{moviesStore.error?.message}</div>
        </div>
    );

});