import { useNavigate, useSearchParams } from 'react-router-dom';
import { getMovies } from '@core/movie/services/api/get-movies';
import { searchMovies } from '@core/movie/services/api/search-movies';
import { useService } from '@app/lib/use-service';
import { Card } from '@app/lib/components/card/card';
import { Pagination } from '@app/lib/components/pagination/pagination';
import { useState } from 'react';
import { MovieCardsSkeleton } from '@app/lib/components/skeleton-cards-list/cards-skeleton-list';
import { ErrorComponent } from '@core/error/components/error-component';
import { scrollToTop } from '@app/utils/scroll-top';
import { NavSection } from './components/nav-section';
import { ContentWrapper, StickyContainer, MoviesContainer, MoviesGrid } from './home.styled';
import { shouldShowPagination } from './utils/should-show-pagination';
import { getMovieImageUrl } from './utils/get-movie-image-url';
import { Movie } from '@core/movie/types/movie';
import { calculateScore } from './utils/calculate-score';

const TMDB_IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w220_and_h330_face';
const DEFAULT_PAGE = 1;

export const Home = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchQuery, setSearchQuery] = useState<string>('');
  const currentPage = Number(searchParams.get('page')) || DEFAULT_PAGE;

  const {
    data: popularMoviesData,
    loading: popularLoading,
    error: popularError,
  } = useService(() => getMovies(currentPage), { dependencies: [currentPage] });

  const {
    data: searchResultsData,
    loading: searchLoading,
    error: searchError,
  } = useService(
    () =>
      searchQuery
        ? searchMovies(searchQuery, currentPage)
        : Promise.resolve({ movies: [], totalPages: 0 }),
    { dependencies: [searchQuery, currentPage] }
  );

  const movies = searchQuery ? searchResultsData?.movies : popularMoviesData?.movies;
  const totalPages = searchQuery ? searchResultsData?.totalPages : popularMoviesData?.totalPages;
  const loading = searchQuery ? searchLoading : popularLoading;
  const error = searchQuery ? searchError : popularError;

  const handleMovieClick = (movieId: number) => {
    navigate(`/movie/${movieId}`);
  };

  const handlePageChange = (page: number) => {
    setSearchParams({ page: page.toString() });
    scrollToTop();
  };

  return (
    <>
      <StickyContainer>
        <NavSection setSearchQuery={setSearchQuery} />
      </StickyContainer>
      <ContentWrapper>
        {error ? (
          <ErrorComponent code={error.code} message={error.message} />
        ) : loading ? (
          <MovieCardsSkeleton />
        ) : !movies?.length ? (
          <h3>No movies found</h3>
        ) : (
          <>
            <MoviesContainer>
              <MoviesGrid>
                {movies.map((movie: Movie) => (
                  <Card key={movie.id} onClick={() => handleMovieClick(movie.id)}>
                    <Card.Image
                      src={getMovieImageUrl(TMDB_IMAGE_BASE_URL, movie)}
                      alt={movie.title}
                      score={calculateScore(movie.voteAverage)}
                    />
                    <Card.Body>
                      <Card.Title>{movie.title}</Card.Title>
                    </Card.Body>
                    <Card.Footer>
                      <small>Release Date: {movie.releaseDate}</small>
                    </Card.Footer>
                  </Card>
                ))}
              </MoviesGrid>
            </MoviesContainer>
            {totalPages && shouldShowPagination(totalPages) ? (
              <Pagination
                currentPage={currentPage}
                totalPages={totalPages}
                onPageChange={handlePageChange}
              />
            ) : null}
          </>
        )}
      </ContentWrapper>
    </>
  );
};
