import React from 'react';
import { useAppSelector } from '../../store';
import Movies from './Movies';
import MovieSearchErrorHandler from './MovieSearchErrorHandler';
import Pagination from './Pagination';

export default function MovieList() {
    const { isFetching, error } = useAppSelector((state) => state.search);

    return (
        <>
            {isFetching ? '... Loading ...' : null}
            <Movies />
            {error ? <MovieSearchErrorHandler /> : null}
            <Pagination />
        </>
    );
}
