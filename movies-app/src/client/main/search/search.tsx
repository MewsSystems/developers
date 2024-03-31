import React from 'react';
import { observer } from 'mobx-react';
import { useInjection } from 'inversify-react';
import { MoviesStore } from '../movies.store';
import { SearchError, SearchInput, SearchWrapper, } from './search.styled';

export const Search = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <SearchWrapper>
            <SearchInput
                type="text"
                placeholder="Search for a movie"
                onChange={ev => moviesStore.searchForString(ev.target.value)}
            />
            <SearchError hidden={moviesStore.error === undefined}>{moviesStore.error?.message}</SearchError>
        </SearchWrapper>
    );

});