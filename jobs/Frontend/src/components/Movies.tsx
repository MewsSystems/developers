import SearchInput from './inputs/SearchInput';
import { useEffect, useState } from 'react';
import { MoviesList } from './MoviesList';
import { useSearchParams } from 'react-router-dom';
import { UrlSearchParamKey } from '../enums/urlSearchParamKey.ts';
import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../enums/reactQueryPrimaryKey.ts';
import { searchMovies, trendingMovies } from '../api/tmdbApi.ts';

export default function Movies() {
    const [searchParams, setSearchParams] = useSearchParams();
    const defaultQuery = searchParams.get(UrlSearchParamKey.Search) ?? '';

    const [query, setQuery] = useState(defaultQuery);
    const [debouncedQuery, setDebouncedQuery] =
        useState(defaultQuery);

    useEffect(() => {
        const timeout = setTimeout(() => {
            if (debouncedQuery === query) return;

            setDebouncedQuery(query);
            setSearchParams(params => {
                params.delete(UrlSearchParamKey.Page);

                if (query) {
                    params.set(UrlSearchParamKey.Search, query);
                } else {
                    params.delete(UrlSearchParamKey.Search);
                }
                return params;
            });
        }, 200);

        return () => clearTimeout(timeout);
    }, [query]);

    const page = getCurrentPage(searchParams);

    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbSearchMovies, query, page],
        queryFn: () => searchMovies(query, page),
        placeholderData: (prev) => prev
    });

    if (isPending && !data) {
        return (<span>Loading</span>);
    }

    if (isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
    }

    // TODO show loading indicator next to search input, only reset movies view once new ones have loaded
    return (
        <>
            <SearchInput query={query} setQuery={setQuery} />
            {
                query
                    ? <MoviesList {...{ data, page }} />
                    : <TrendingMovies page={page} />
            }
        </>
    );
}

function TrendingMovies({ page }: { page: number }) {
    const timeWindow = 'week';
    const { data, isError, isPending } = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbTendingMovies, timeWindow, page],
        queryFn: () => trendingMovies(timeWindow, page)
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
        results
    } = data;

    return (
        <>
            <h2>Top trending this week</h2>
            <MoviesList data={{ results: results.slice(0, 8), totalPages: 1, totalResults: 1, page: 1 }} page={page}/>
        </>
    );
}


const getCurrentPage = (searchParams: URLSearchParams) => {
    const page = searchParams.has(UrlSearchParamKey.Page)
        ? Number(searchParams.get(UrlSearchParamKey.Page)) : 1;

    return isNaN(page) || page <= 1
        ? 1 : page;
}