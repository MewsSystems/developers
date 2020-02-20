import React from 'react';
import { useDispatch } from 'react-redux';
import { searchMovieAction } from './store/search-movie-actions';
import { useAppSelector } from '../../store';
import MovieList from './MovieList';

export default function MovieListFetcher() {
    const dispatch = useDispatch();
    const { query, page } = useAppSelector((state) => state.search);

    React.useEffect(() => {
        if (query) {
            dispatch(searchMovieAction());
        }
    }, [query, page]);

    return (<MovieList />);
}
