import React from 'react';
import SearchInput from '../modules/movie-search/SearchInput';
import MovieListFetcher from '../modules/movie-search/MovieListFetcher';
import { useAppSelector } from '../store';

export default function SearchPage() {
    const { query } = useAppSelector((state) => state.search);

    return (
        <>
            <SearchInput defaultQuery={query} />
            <MovieListFetcher />
        </>
    );
}
