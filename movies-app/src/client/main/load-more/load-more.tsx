import React from 'react';
import { observer } from 'mobx-react';
import { useInjection } from 'inversify-react';
import { MoviesStore } from '../movies.store';
import { LoadMoreWrapper, LoadMoreButton } from './load-more.styled';

export const LoadMore = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    if (moviesStore.movies.length === 0 || moviesStore.lastOfCurrentPages === moviesStore.totalPages) {
        return null;
    }

    const loadMore = () => {
        moviesStore.loadMore();
    }


    return (
        <LoadMoreWrapper>
            <LoadMoreButton onClick={loadMore}>
                Load more
            </LoadMoreButton>
        </LoadMoreWrapper>
    );

});