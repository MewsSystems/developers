import { useQuery } from '@tanstack/react-query';
import { searchMovies } from '../api/tmdbApi';
import { ReactQueryPrimaryKey } from '../enums/reactQueryPrimaryKey';
import { useSearchParams } from 'react-router-dom';
import { Pagination } from './Pagination';
import { UrlSearchParamKey } from '../enums/urlSearchParamKey';
import { MoviesListUl } from './MoviesList.styled';
import { MovieListItem } from './MovieListItem.tsx';

export function MoviesList({query}: { query: string }) {
    // TODO: debounce query
    // TODO: don't render when no query set
    // TODO: add pagination

    const [searchParams, _] = useSearchParams();
    const page = getCurrentPage(searchParams);

    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbSearchMovies, query, page],
        queryFn: () => searchMovies(query, page)
    });

    if (isPending) {
        return (<span>Loading</span>);
    }

    if (isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
    }

    const {
        results,
        totalPages
    } = data;

    console.log(results);

    const movieListItems = results.map(movie => MovieListItem(movie));

    const showPagination = totalPages > 1;

    return (
        <>
            <MoviesListUl>
                {movieListItems}
            </MoviesListUl>
            {showPagination && <Pagination currentPage={page} numberOfPages={totalPages} />}
        </>
    );
}

const getCurrentPage = (searchParams: URLSearchParams) => {
    const page = searchParams.has(UrlSearchParamKey.Page)
        ? Number(searchParams.get(UrlSearchParamKey.Page)) : 1;

    return isNaN(page) || page <= 1
        ? 1 : page;
}