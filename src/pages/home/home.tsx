import { useNavigate, useSearchParams } from 'react-router-dom';
import { getMovies, searchMovies } from "@movie/services/api/movie-api";
import { useService } from "@app/lib/use-service";
import { Card } from "@app/lib/components/card/card";
import { Pagination } from "@app/lib/components/pagination/pagination";
import { useState } from 'react';
import { MovieCardsSkeleton } from '@app/lib/components/skeleton-cards-list/cards-skeleton-list';
import { ErrorComponent } from '@core/error/components/error-component';
import { scrollToTop } from '@app/utils/scroll-top';
import { NavSection } from './components/nav-section';
import { ContentWrapper, StickyContainer, MoviesContainer, MoviesGrid } from './home.styled';

export const Home: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchQuery, setSearchQuery] = useState<string>('');
  const currentPage = Number(searchParams.get('page')) || 1;
  
  const { data: popularMoviesData, loading: popularLoading, error: popularError } = useService(
    () => getMovies(currentPage),
    { dependencies: [currentPage] }
  );

  const { data: searchResultsData, loading: searchLoading, error: searchError } = useService(
    () => searchQuery ? searchMovies(searchQuery, currentPage) : Promise.resolve({ movies: [], totalPages: 0 }),
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
                {movies.map((movie) => (
                  <Card 
                    key={movie.id}
                    onClick={() => handleMovieClick(movie.id)}
                  >
                    <Card.Image 
                       src={`https://image.tmdb.org/t/p/w220_and_h330_face${movie.posterPath}`}
                       alt={movie.title}
                       loading="lazy"
                       score={Math.floor(movie.voteAverage * 10)}
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
            {totalPages && totalPages > 1 && (
              <Pagination
                currentPage={currentPage}
                totalPages={totalPages}
                onPageChange={handlePageChange}
              />
            )}
          </>
        )}
      </ContentWrapper>
    </>
  );
}; 