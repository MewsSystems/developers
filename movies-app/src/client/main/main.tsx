import React from 'react';
import {Search} from "./search";

export const MainPage = () => {
    return (
        <main className="main">
            <div className="search">
                <Search />
            </div>
            <div className="pagination">
                pages
            </div>
            <div className="movies-list">
                movies
            </div>
        </main>
    );
};