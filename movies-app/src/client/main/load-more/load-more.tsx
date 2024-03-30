import React, { Fragment } from 'react';
import {observer} from "mobx-react";
import {useInjection} from "inversify-react";
import classNames from 'classnames';
import {MoviesStore} from "../movies.store";
import './load-more.scss';

export const LoadMore = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    if (moviesStore.movies.length === 0 || moviesStore.lastOfCurrentPages === moviesStore.totalPages) {
        return null;
    }

    const loadMore = () => {
        moviesStore.loadMore();
    }


    return (
        <div className="load-more">
            <div className="load-more__button" onClick={loadMore}>
                Load more
            </div>
        </div>
    );

});