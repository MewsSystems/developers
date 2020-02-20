import React from 'react';
import { useDispatch } from 'react-redux';
import { getMovieAction } from './store/detail-movie-actions';
import ErrorHandler from '../../components/ErrorHandler';

export default function MovieDetailErrorHandler({ id }: { id: number }) {
    const dispatch = useDispatch();

    return (
        <ErrorHandler
            tryItAgain={() => {
                dispatch(getMovieAction(id));
            }}
        />
    );
}
