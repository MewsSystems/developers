import React from 'react';
import {Search} from "./search/search";
import { Pagination } from "./pagination/pagination";
import { MoviesList } from "./movies-list/movies-list";
import { LoadMore } from "./load-more/load-more";
import './main.scss';

export const MainPage = () => {
    return (
        <main className="main">
            <div className="main__search">
                <Search />
            </div>
            <div className="main__pagination">
                <Pagination />
            </div>
            <div className="main__movies-list">
                <MoviesList />
            </div>
            <div className="main__load-more">
                <LoadMore />
            </div>
        </main>
    );
};