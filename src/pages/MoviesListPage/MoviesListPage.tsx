import {useDebounce} from '../../hooks/useDebounce';
import {useSearchInput} from '../../hooks/useSearchInput';
import {useMoviesList} from '../../hooks/useMoviesList';
import {usePaginationWithUrlSync} from '../../hooks/usePaginationWithUrlSync';
import {getErrorFallbackMessage} from '../../api/movieApi/utils/getErrorFallbackMessage';
import ApiErrorScreen from '../../app/components/ApiErrorScreen/ApiErrorScreen';
import PopcornLoader from '../common/PopcornLoader/PopcornLoader';
import EmptyInitialState from './components/EmptyInitialState/EmptyInitialState';
import EmptySearchResultState from './components/EmptySearchResultState/EmptySearchResultState';
import MovieCard from './components/MovieCard/MovieCard';
import Pagination from './components/Pagination/Pagination';
import SearchBar, {MAX_USER_INPUT_SEARCH_LENGTH} from './components/SearchInput/SearchInput';
import {Container, Content, Header, LoadingOverlay, MoviesContainer, MoviesGrid} from './styled';

export default function MoviesListPage() {
  const {searchUrlParam} = useSearchInput();
  const debouncedSearchQuery = useDebounce(searchUrlParam);

  const {movies, totalPages, error, isLoading, isFetching, clearMoviesCache} = useMoviesList(
    debouncedSearchQuery,
    1,
  );

  const {currentPage, setPage} = usePaginationWithUrlSync({
    totalPages,
    searchQuery: debouncedSearchQuery,
  });

  const currentPageData = useMoviesList(debouncedSearchQuery, currentPage);

  const displayMovies = currentPage === 1 ? movies : currentPageData.movies;
  const displayError = currentPage === 1 ? error : currentPageData.error;
  const isPageLoading = currentPage === 1 ? isLoading : currentPageData.isLoading;
  const isPageFetching = currentPage === 1 ? isFetching : currentPageData.isFetching;

  const shouldShowInitialEmptyState = !isPageLoading && !debouncedSearchQuery;
  const hasEmptySearchResults =
    !isPageLoading &&
    displayMovies.length === 0 &&
    debouncedSearchQuery &&
    debouncedSearchQuery.length !== MAX_USER_INPUT_SEARCH_LENGTH;

  if (displayError) {
    return (
      <ApiErrorScreen
        errorMessage={getErrorFallbackMessage({
          status: displayError.status,
          message: displayError.message,
        })}
        onReset={clearMoviesCache}
      />
    );
  }

  return (
    <Container>
      <Header>
        <SearchBar />
      </Header>

      <Content>
        {isPageLoading && !isPageFetching && (
          <LoadingOverlay>
            <PopcornLoader />
          </LoadingOverlay>
        )}
        {shouldShowInitialEmptyState && <EmptyInitialState />}
        {hasEmptySearchResults && <EmptySearchResultState />}

        <MoviesContainer>
          {isPageFetching && !isPageLoading && !hasEmptySearchResults && (
            <LoadingOverlay>
              <PopcornLoader />
            </LoadingOverlay>
          )}

          <MoviesGrid>
            {displayMovies.map((movie) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </MoviesGrid>
        </MoviesContainer>

        <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setPage} />
      </Content>
    </Container>
  );
}

MoviesListPage.displayName = 'MoviesListPage';
