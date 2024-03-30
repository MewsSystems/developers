import React, { Fragment } from 'react';
import { observer } from "mobx-react";
import { useInjection } from "inversify-react";
import { Search } from "./search/search";
import { Pagination } from "./pagination/pagination";
import { MoviesList } from "./movies-list/movies-list";
import { LoadMore } from "./load-more/load-more";
import { MoviesStore } from "./movies.store";
import { MainWrapper, LoadMoreWrapper, MoviesListWrapper, PaginationWrapper, SearchWrapper } from './main.styled';

export const MainPage = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <MainWrapper>
            <SearchWrapper>
                <Search/>
            </SearchWrapper>
            {
                moviesStore.noResultsFound
                    ? <div>No results found</div>
                    : (
                        <Fragment>
                            <PaginationWrapper>
                                <Pagination/>
                            </PaginationWrapper>
                            <MoviesListWrapper>
                                <MoviesList/>
                            </MoviesListWrapper>
                            <LoadMoreWrapper>
                                <LoadMore/>
                            </LoadMoreWrapper>
                        </Fragment>
                    )
            }
        </MainWrapper>
    );
});