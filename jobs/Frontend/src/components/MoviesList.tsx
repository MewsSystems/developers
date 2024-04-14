import { useQuery } from '@tanstack/react-query';
import { searchMovies } from '../movieDbUtils';

export function MoviesList({query}: { query: string }) {
    // TODO: debounce query
    // TODO: don't render when no query set
    // TODO: add pagination

    const movies = useQuery({
        queryKey: [query],
        queryFn: () => searchMovies(query)
    });

    if (movies.isPending) {
        return (<span>Loading</span>);
    }

    if (movies.isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
    }

    const movieListItems = movies.data.results.map(movie => (
        <li key={movie.id}>
            {movie.title} <i>({movie.vote_average})</i>
        </li>
    ));

    return (
        <ul>
            {movieListItems}
        </ul>
    );
}