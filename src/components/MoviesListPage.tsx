import {useQuery} from '@tanstack/react-query';
import {useEffect} from 'react';
import {Link, useLocation, useSearchParams} from 'react-router-dom';
import {useDebounce} from '../hooks/useDebounce';
import {usePagination} from '../hooks/usePagination';
import {useSearchInput} from '../hooks/useSearchInput.ts';
import {fetchMoviesList} from '../api/fetchMoviesList.ts';

const MoviesListPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const location = useLocation();
  const {page, onPageChange} = usePagination();
  const {searchUrlParam, onSearchInputChange} = useSearchInput();
  const debouncedSearchQuery = useDebounce(searchUrlParam);

  const {data, isLoading} = useQuery({
    queryKey: ['movies', debouncedSearchQuery, page],
    queryFn: () => fetchMoviesList(debouncedSearchQuery, page),
    enabled: debouncedSearchQuery.length > 0,
  });

  const {total_pages: totalPagesCount, results: movies} = data || {};

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

    // Add page=1 only if there are multiple pages and we're not already on a page
    if (totalPagesCount && totalPagesCount > 1 && !currentPageUrlQueryParam) {
      newParams.set('page', '1');
      setSearchParams(newParams);
    }
    // Remove "page" URL param if there's only one page
    else if (totalPagesCount && totalPagesCount <= 1 && currentPageUrlQueryParam) {
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

      {isLoading && <div>Loading...</div>}

      {movies?.map((movie) => (
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

      {totalPagesCount && totalPagesCount > 1 && (
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
};

MoviesListPage.displayName = 'MoviesListPage';
export default MoviesListPage;
