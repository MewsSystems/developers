import { useQuery } from '@tanstack/react-query';
import { searchMovies } from '../api/tmdbApi.ts';
import { ReactQueryPrimaryKey } from '../enums/reactQueryPrimaryKey.ts';
import { useEffect, useState } from 'react';

export function MoviesList({query}: { query: string }) {
    // TODO: debounce query
    // TODO: don't render when no query set
    // TODO: add pagination

    const [debouncedQuery, setDebouncedQuery] = useState('');

    useEffect(() => {
       const timeout = setTimeout(() => {
           setDebouncedQuery(query);
       }, 200);

       return () => clearTimeout(timeout);
    }, [query]);

    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.SearchMovies, debouncedQuery],
        queryFn: () => searchMovies(debouncedQuery)
    });

    if (isPending) {
        return (<span>Loading</span>);
    }

    if (isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
    }

    const movieListItems = data.results.map(movie => (
        <li key={movie.id}>
            {movie.title} <i>({movie.voteAverage})</i>
            <p>{movie.overview}</p>
        </li>
    ));

    return (
        <ul>
            {movieListItems}
        </ul>
    );
}