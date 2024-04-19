import SearchInput from '../../inputs/SearchInput';
import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { MoviesList } from './MoviesList';
import { useSearchParams } from 'react-router-dom';
import { UrlSearchParamKey } from '../../../enums/urlSearchParamKey';
import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../../../enums/reactQueryPrimaryKey';
import { searchMovies } from '../../../api/tmdbApi';
import { Loader } from '../../shared/Loader';
import { TrendingMovies } from './TrendingMovies';

export default function Movies() {
    const [debouncedQuery, page, query, setQuery] = useMoviesData();

    const enabled = !!debouncedQuery;

    const {data, isError, isPending, isFetching} = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbSearchMovies, debouncedQuery, page],
        queryFn: () => searchMovies(debouncedQuery, page),
        placeholderData: (prev) => prev,
        enabled
    });

    const search = (
        <>
            <SearchInput label="Search movies" {...{query, setQuery}}/>
            {isFetching && <Loader/>}
        </>
    );

    if (isPending) {
        return (
            <>
                {search}
                <TrendingMovies page={page}/>
            </>
        );
    }

    if (isError) {
        return (
            <span>Failed to load data, please reload this page or came back later</span>
        );
    }

    return (
        <>
            {search}
            {
                query
                    ? <MoviesList {...{data, page}} />
                    : <TrendingMovies page={page}/>
            }
        </>
    );
}

type UseMoviesDataResult = [debouncedQuery: string, page: number, query: string, setQuery: Dispatch<SetStateAction<string>>];
const useMoviesData = (): UseMoviesDataResult => {
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

        return () => { clearTimeout(timeout); };
    }, [debouncedQuery, query, setSearchParams]);

    const page = getCurrentPage(searchParams);

    return [debouncedQuery, page, query, setQuery];
};


const getCurrentPage = (searchParams: URLSearchParams) => {
    const page = searchParams.has(UrlSearchParamKey.Page)
        ? Number(searchParams.get(UrlSearchParamKey.Page)) : 1;

    return isNaN(page) || page <= 1
        ? 1 : page;
};