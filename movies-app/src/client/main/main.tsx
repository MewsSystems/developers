import React from 'react';

export const MainPage = () => {
    return (
        <main className="main">
            <div className="search">
                <input type="text" placeholder="Search for a movie" />
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