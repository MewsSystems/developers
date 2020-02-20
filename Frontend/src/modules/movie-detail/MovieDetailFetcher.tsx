import React from 'react';
import { useDispatch } from 'react-redux';
import MovieDetail from './MovieDetail';
import MovieDetailErrorHandler from './MovieDetailErrorHandler';
import { getMovieAction } from './store/detail-movie-actions';
import { useAppSelector } from '../../store';

interface Props {
    id: number,
}

export default function MovieDetailFetcher({ id }: Props) {
    const dispatch = useDispatch();
    const { movie, error } = useAppSelector((state) => state.detail);

    React.useEffect(() => {
        dispatch(getMovieAction(id));
    }, []);

    if (error) {
        return <MovieDetailErrorHandler id={id} />;
    }

    if (movie) {
        return <MovieDetail {...movie} />;
    }

    return (<>...Loading...</>);
}
