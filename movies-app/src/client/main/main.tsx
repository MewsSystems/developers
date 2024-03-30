import React, { Fragment } from 'react';
import { observer } from "mobx-react";
import { useInjection } from "inversify-react";
import { Search } from "./search/search";
import { Pagination } from "./pagination/pagination";
import { MoviesList } from "./movies-list/movies-list";
import { LoadMore } from "./load-more/load-more";
import { MoviesStore } from "./movies.store";
import './main.scss';

export const MainPage = observer(() => {
    const moviesStore = useInjection(MoviesStore);

    return (
        <main className="main">
            <div className="main__search">
                <Search/>
            </div>
            {
                moviesStore.noResultsFound
                    ? <div className="main__no-results">No results found</div>
                    : (
                        <Fragment>
                            <div className="main__pagination">
                                <Pagination/>
                            </div>
                            <div className="main__movies-list">
                                <MoviesList/>
                            </div>
                            <div className="main__load-more">
                                <LoadMore/>
                            </div>
                        </Fragment>
                    )
            }
        </main>
    );
});