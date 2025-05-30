import {useQuery} from '@tanstack/react-query';
import {useEffect} from 'react';
import {useSearchParams} from 'react-router-dom';
import {useDebounce} from '../../hooks/useDebounce.ts';
import {usePagination} from '../../hooks/usePagination.ts';
import {useSearchInput} from '../../hooks/useSearchInput.ts';
import {fetchMoviesList} from '../../api/fetchMoviesList.ts';
import LoadingCameraAnimation from './components/LoadingCameraAnimation/LoadingCameraAnimation.tsx';
import NothingFoundState from './components/EmptySearchResult/NothingFoundState.tsx';
import EmptyInitialState from './components/EmptyInitialState/EmptyInitialState.tsx';
import MovieCard from './components/MovieCard/MovieCard.tsx';
import Pagination from './components/Pagination/Pagination.tsx';
import type {MovieSearchResponse} from '../../api/types';
import {
  Container,
  Content,
  Header,
  MoviesGrid,
  LoadingOverlay,
  MoviesContainer,
} from './MoviesListPage.styled.tsx';
import SearchBar, {MAX_USER_INPUT_SEARCH_LENGTH} from './components/SearchInput/SearchInput.tsx';

export default function MoviesListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const {page, onPageChange} = usePagination();
  const {searchUrlParam} = useSearchInput();
  const debouncedSearchQuery = useDebounce(searchUrlParam);

  const FIVE_MINUTES = 5 * 60 * 1000;

  /*
    1. staleTime: avoid re-fetching the same search results for 5 minutes — treat them as fresh during this time.
    2. gcTime: keep the cached results in memory for 10 minutes after they're no longer used. Could be increased
    since movie data doesn't change frequently
  */
  const {data, isLoading, isFetching} = useQuery<MovieSearchResponse>({
    queryKey: ['movies', debouncedSearchQuery, page],
    queryFn: () => fetchMoviesList(debouncedSearchQuery, page),
    enabled:
      debouncedSearchQuery.length > 0 &&
      debouncedSearchQuery.length <= MAX_USER_INPUT_SEARCH_LENGTH,
    staleTime: FIVE_MINUTES,
    gcTime: FIVE_MINUTES * 2,
    placeholderData: (previousData) => {
      if (!debouncedSearchQuery) {
        return undefined;
      }

      return previousData;
    },
  });

  const {total_pages = 0, results = []} = data || {};
  const shouldShowInitialEmptyState = !isLoading && !debouncedSearchQuery;

  const hasEmptySearchResults =
    !isLoading &&
    results.length === 0 &&
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
    if (total_pages > 1 && !currentPageUrlQueryParam) {
      newParams.set('page', '1');
      setSearchParams(newParams);
    }
    // Remove "&page=X" URL param if there's only one page
    else if (total_pages <= 1 && currentPageUrlQueryParam) {
      newParams.delete('page');
      setSearchParams(newParams);
    }
  }, [data, debouncedSearchQuery, searchParams, setSearchParams, total_pages]);

  return (
    <Container>
      <Header>
        <SearchBar />
      </Header>

      <Content>
        {shouldShowInitialEmptyState && <EmptyInitialState />}
        {hasEmptySearchResults && <NothingFoundState />}

        <MoviesContainer>
          {isFetching && !hasEmptySearchResults && (
            <LoadingOverlay>
              <LoadingCameraAnimation />
            </LoadingOverlay>
          )}

          <MoviesGrid>
            {results.map((movie) => (
              <MovieCard
                key={movie.id}
                movie={movie}
                searchParams={searchParams}
                searchQuery={searchUrlParam}
                currentPage={page}
              />
            ))}
          </MoviesGrid>
        </MoviesContainer>

        <Pagination currentPage={page} totalPages={total_pages} onPageChange={onPageChange} />
      </Content>
    </Container>
  );
}

MoviesListPage.displayName = 'MoviesListPage';
