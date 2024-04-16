import SearchInput from './inputs/SearchInput';
import { useEffect, useState } from 'react';
import { MoviesList } from './MoviesList';
import { useSearchParams } from 'react-router-dom';
import { UrlSearchParamKey } from '../enums/urlSearchParamKey.ts';

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

    // TODO show loading indicator next to search input, only reset movies view once new ones have loaded
    return (
        <>
            <SearchInput query={query} setQuery={setQuery} />
            { query
                ? <MoviesList query={debouncedQuery} />
                : <h2>Start typing and search results will appear</h2>
            }
        </>
    );
}

