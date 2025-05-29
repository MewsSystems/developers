import {useQuery} from '@tanstack/react-query';
import {useEffect} from 'react';
import {Link, useLocation, useSearchParams} from 'react-router-dom';
import {useDebounce} from '../../hooks/useDebounce.ts';
import {usePagination} from '../../hooks/usePagination.ts';
import {useSearchInput} from '../../hooks/useSearchInput.ts';
import {fetchMoviesList} from '../../api/fetchMoviesList.ts';
import {LoadingSkeleton} from './LoadingSkeleton.tsx';
import type {MovieSearchResponse} from '../../api/types';

export default function MoviesListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const location = useLocation();
  const {page, onPageChange} = usePagination();
  const {searchUrlParam, onSearchInputChange} = useSearchInput();
  const debouncedSearchQuery = useDebounce(searchUrlParam);

  const FIVE_MINUTES = 5 * 60 * 1000;

  const {data, isLoading} = useQuery<MovieSearchResponse>({
    queryKey: ['movies', debouncedSearchQuery, page],
    queryFn: () => fetchMoviesList(debouncedSearchQuery, page),
    enabled: debouncedSearchQuery.length > 0,
    // Avoid re-fetching the same search results for 5 minutes — treat them as fresh during this time
    staleTime: FIVE_MINUTES,
    // Keep the cached results in memory for 10 minutes after they're no longer used
    // Could be increased since movie data doesn't change frequently
    gcTime: FIVE_MINUTES * 2,
  });

  const {total_pages: totalPagesCount = 0, results: movies = []} = data || {};

  const isFirstPage = page === 1;
  const isLastPage = page === totalPagesCount;

  useEffect(() => {
    if (!data) {
      // If there's no data and no search query, remove the page parameter
      if (!debouncedSearchQuery && searchParams.has('page')) {
        const newParams = new URLSearchParams(searchParams);
        newParams.delete('page');
        setSearchParams(newParams);
      }
      return;
    }

    const newParams = new URLSearchParams(searchParams);
    const currentPageUrlQueryParam = searchParams.get('page');

    // Add &page=1 only if there are multiple pages and we're not already on a page
    if (totalPagesCount > 1 && !currentPageUrlQueryParam) {
      newParams.set('page', '1');
      setSearchParams(newParams);
    }
    // Remove "page" URL param if there's only one page
    else if (totalPagesCount <= 1 && currentPageUrlQueryParam) {
      newParams.delete('page');
      setSearchParams(newParams);
    }
  }, [data, debouncedSearchQuery, searchParams, setSearchParams, totalPagesCount]);

  return (
    <div>
      <input
        type="text"
        value={searchUrlParam}
        onChange={onSearchInputChange}
        placeholder="Search movies..."
      />

      {isLoading && <LoadingSkeleton />}

      {movies.map((movie) => (
        <div key={movie.id}>
          <Link
            to={`/movies/${movie.id}?${searchParams.toString()}`}
            state={{
              from: location.pathname,
              search: searchUrlParam,
              page: page,
            }}
          >
            <h3>{movie.title}</h3>
            <p>{movie.overview}</p>
          </Link>
        </div>
      ))}

      {totalPagesCount > 1 && (
        <div>
          <button onClick={() => onPageChange(page - 1)} disabled={isFirstPage}>
            Previous
          </button>
          <span>
            Page {page} of {totalPagesCount}
          </span>
          <button onClick={() => onPageChange(page + 1)} disabled={isLastPage}>
            Next
          </button>
        </div>
      )}
    </div>
  );
}

MoviesListPage.displayName = 'MoviesListPage';
