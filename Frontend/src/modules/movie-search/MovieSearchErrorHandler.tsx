import React from 'react';
import { useDispatch } from 'react-redux';
import { searchMovieAction } from './store/search-movie-actions';
import ErrorHandler from '../../components/ErrorHandler';

export default function MovieSearchErrorHandler() {
    const dispatch = useDispatch();

    return (
        <ErrorHandler
            tryItAgain={() => {
                dispatch(searchMovieAction());
            }}
        />
    );
}
