import {useQuery, useQueryClient} from '@tanstack/react-query';
import {useEffect} from 'react';
import {useSearchParams} from 'react-router-dom';
import {useDebounce} from '../../hooks/useDebounce';
import {usePagination} from '../../hooks/usePagination';
import {useSearchInput} from '../../hooks/useSearchInput';
import {fetchMoviesList} from '../../api/movieApi/endpoints/fetchMoviesList';
import {getErrorFallbackMessage} from '../../api/movieApi/utils/getErrorFallbackMessage';
import ApiErrorScreen from '../../app/components/ApiErrorScreen/ApiErrorScreen';
import PopcornLoader from '../common/PopcornLoader/PopcornLoader';
import NothingFoundState from './components/EmptySearchResult/NothingFoundState';
import EmptyInitialState from './components/EmptyInitialState/EmptyInitialState';
import MovieCard from './components/MovieCard/MovieCard';
import Pagination from './components/Pagination/Pagination';
import SearchBar, {MAX_USER_INPUT_SEARCH_LENGTH} from './components/SearchInput/SearchInput';
import {Container, Content, Header, MoviesGrid, LoadingOverlay, MoviesContainer} from './styled';
import type {MovieSearchResponse} from '../../api/movieApi/types';
import type {ApiErrorResponseDetails} from '../../api/movieApi/utils/types';

export default function MoviesListPage() {
  const queryClient = useQueryClient();
  const [searchParams, setSearchParams] = useSearchParams();
  const {page, onPageChange} = usePagination();
  const {searchUrlParam} = useSearchInput();
  const debouncedSearchQuery = useDebounce(searchUrlParam);

  const FIVE_MINUTES = 5 * 60 * 1000;

  const {data, error, isLoading, isFetching} = useQuery<
    MovieSearchResponse,
    ApiErrorResponseDetails
  >({
    queryKey: ['movies', debouncedSearchQuery, page],
    queryFn: () => fetchMoviesList(debouncedSearchQuery, page),
    enabled:
      debouncedSearchQuery.length > 0 &&
      debouncedSearchQuery.length <= MAX_USER_INPUT_SEARCH_LENGTH,
    staleTime: FIVE_MINUTES,
    gcTime: FIVE_MINUTES * 2,
    placeholderData: (previousItems) => (debouncedSearchQuery ? previousItems : undefined),
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
        const currentUrlSearchParams = new URLSearchParams(searchParams);
        currentUrlSearchParams.delete('page');
        setSearchParams(currentUrlSearchParams);
      }

      return;
    }

    const currentUrlSearchParams = new URLSearchParams(searchParams);
    const currentPageUrlQueryParam = searchParams.get('page');

    // Add "&page=1" only if the total pages count > 1 and the user is not already on a page
    if (total_pages > 1 && !currentPageUrlQueryParam) {
      currentUrlSearchParams.set('page', '1');
      setSearchParams(currentUrlSearchParams);
    }
    // Remove "&page=X" URL param if there's only one page
    else if (total_pages <= 1 && currentPageUrlQueryParam) {
      currentUrlSearchParams.delete('page');
      setSearchParams(currentUrlSearchParams);
    }
  }, [data, debouncedSearchQuery, searchParams, setSearchParams, total_pages]);

  if (error) {
    return (
      <ApiErrorScreen
        errorMessage={getErrorFallbackMessage({status: error.status, message: error.message})}
        onReset={() => {
          queryClient.clear();
        }}
      />
    );
  }

  return (
    <Container>
      <Header>
        <SearchBar />
      </Header>

      <Content>
        {isLoading && !isFetching && (
          <LoadingOverlay>
            <PopcornLoader />
          </LoadingOverlay>
        )}
        {shouldShowInitialEmptyState && <EmptyInitialState />}
        {hasEmptySearchResults && <NothingFoundState />}

        <MoviesContainer>
          {isFetching && !isLoading && !hasEmptySearchResults && (
            <LoadingOverlay>
              <PopcornLoader />
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
