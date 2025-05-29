import {useQuery} from '@tanstack/react-query';
import {useEffect, useRef} from 'react';
import {useSearchParams} from 'react-router-dom';
import {useDebounce} from '../../hooks/useDebounce.ts';
import {usePagination} from '../../hooks/usePagination.ts';
import {useSearchInput} from '../../hooks/useSearchInput.ts';
import {fetchMoviesList} from '../../api/fetchMoviesList.ts';
import {LoadingSkeleton} from './components/LoadingSkeleton/LoadingSkeleton.tsx';
import {NothingFoundState} from './components/EmptySearchResult/NothingFoundState.tsx';
import {EmptyInitialState} from './components/EmptyInitialState/EmptyInitialState.tsx';
import {MovieCard} from './components/MovieCard/MovieCard.tsx';
import {Pagination} from './components/Pagination/Pagination.tsx';
import type {MovieSearchResponse} from '../../api/types';
import {
  Container,
  Content,
  Header,
  MoviesGrid,
  SearchContainer,
  SearchInput,
  SearchWarning,
} from './MoviesListPage.styled.tsx';

export default function MoviesListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const {page, onPageChange} = usePagination();
  const {searchUrlParam, onSearchInputChange} = useSearchInput();
  const debouncedSearchQuery = useDebounce(searchUrlParam);
  const searchInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    searchInputRef.current?.focus();
  }, []);

  /*
    "MAX_USER_INPUT_SEARCH_LENGTH" search query has been limited to 100 chars since most movie titles
    are well under this length. This helps to prevent abusing the API.
  */
  const MAX_USER_INPUT_SEARCH_LENGTH = 100;
  const FIVE_MINUTES = 5 * 60 * 1000;
  const SEARCH_INPUT_WARNING_THRESHOLD = 90;

  /*
    1. staleTime: avoid re-fetching the same search results for 5 minutes — treat them as fresh during this time.
    2. gcTime: keep the cached results in memory for 10 minutes after they're no longer used. Could be increased
    since movie data doesn't change frequently
  */
  const {data, isLoading} = useQuery<MovieSearchResponse>({
    queryKey: ['movies', debouncedSearchQuery, page],
    queryFn: () => fetchMoviesList(debouncedSearchQuery, page),
    enabled:
      debouncedSearchQuery.length > 0 &&
      debouncedSearchQuery.length <= MAX_USER_INPUT_SEARCH_LENGTH,
    staleTime: FIVE_MINUTES,
    gcTime: FIVE_MINUTES * 2,
  });

  const {total_pages: totalPagesCount = 0, results: movies = []} = data || {};

  const charactersLeft = MAX_USER_INPUT_SEARCH_LENGTH - searchUrlParam.length;
  const shouldShowInitialEmptyState = !isLoading && !debouncedSearchQuery;

  const hasEmptySearchResults =
    !isLoading &&
    movies.length === 0 &&
    debouncedSearchQuery &&
    debouncedSearchQuery.length !== MAX_USER_INPUT_SEARCH_LENGTH;

  useEffect(() => {
    if (!data) {
      // If there's no data and no search query, remove the "page" query parameter from the URL
      if (!debouncedSearchQuery && searchParams.has('page')) {
        const newParams = new URLSearchParams(searchParams);
        newParams.delete('page');
        setSearchParams(newParams);
      }

      return;
    }

    const newParams = new URLSearchParams(searchParams);
    const currentPageUrlQueryParam = searchParams.get('page');

    // Add "&page=1" only if the total pages count > 1 and the user is not already on a page
    if (totalPagesCount > 1 && !currentPageUrlQueryParam) {
      newParams.set('page', '1');
      setSearchParams(newParams);
    }
    // Remove "&page=X" URL param if there's only one page
    else if (totalPagesCount <= 1 && currentPageUrlQueryParam) {
      newParams.delete('page');
      setSearchParams(newParams);
    }
  }, [data, debouncedSearchQuery, searchParams, setSearchParams, totalPagesCount]);

  return (
    <Container>
      <Header>
        <SearchContainer>
          <SearchInput
            ref={searchInputRef}
            type="text"
            value={searchUrlParam}
            onChange={onSearchInputChange}
            placeholder="Start typing..."
            maxLength={MAX_USER_INPUT_SEARCH_LENGTH}
          />
          {searchUrlParam.length >= SEARCH_INPUT_WARNING_THRESHOLD && (
            <SearchWarning isError={charactersLeft === 0}>
              {charactersLeft === 0
                ? "You've reached the limit of 100 characters."
                : `${charactersLeft} characters remaining`}
            </SearchWarning>
          )}
        </SearchContainer>
      </Header>

      <Content>
        {isLoading && <LoadingSkeleton />}
        {shouldShowInitialEmptyState && <EmptyInitialState />}
        {hasEmptySearchResults && <NothingFoundState />}

        {!isLoading && movies.length > 0 && (
          <MoviesGrid>
            {movies.map((movie) => (
              <MovieCard
                key={movie.id}
                movie={movie}
                searchParams={searchParams}
                searchQuery={searchUrlParam}
                currentPage={page}
              />
            ))}
          </MoviesGrid>
        )}

        <Pagination currentPage={page} totalPages={totalPagesCount} onPageChange={onPageChange} />
      </Content>
    </Container>
  );
}

MoviesListPage.displayName = 'MoviesListPage';
