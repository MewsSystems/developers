import { useNavigate, useSearchParams } from 'react-router-dom';
import { getMovies, searchMovies } from "@movie/services/api/movie-api";
import { useService } from "@app/lib/useService";
import { Card } from "@app/lib/components/card/card";
import { Search } from "@app/lib/components/search/search";
import { Pagination } from "@app/lib/components/pagination/pagination";
import { useState } from 'react';
import { MovieCardsSkeleton } from '@app/lib/components/skeleton-cards-list/skeleton-cards-list';
import { ErrorComponent } from '@core/error/components/error-component';
import styled from 'styled-components';

const ResetButton = styled.button`
  position: fixed;
  top: 1rem;
  right: 1rem;
  padding: 0.75rem 1.5rem;
  background: #007bff;
  border: none;
  border-radius: 8px;
  color: white;
  cursor: pointer;
  transition: all 0.2s ease;
  font-weight: 500;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  z-index: 100;

  &:hover {
    background: #0056b3;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  }

  &:active {
    transform: translateY(0);
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }
`;

export const Home: React.FC = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [searchQuery, setSearchQuery] = useState('');
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
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const handleReset = () => {
    setSearchQuery('');
    setSearchParams({});
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  return (
    <>
      <ResetButton onClick={handleReset}>
        Home
      </ResetButton>
      <Search onSearch={setSearchQuery} />
      {error ? (
        <ErrorComponent code={error.code} message={error.message} />
      ) : loading ? (
        <MovieCardsSkeleton />
      ) : !movies?.length ? (
        <h3>No movies found</h3>
      ) : (
        <>
          <div style={{ padding: '2rem' }}>
            <div style={{ 
              display: 'grid',
              gridTemplateColumns: 'repeat(auto-fill, minmax(180px, 1fr))',
              gap: '2rem',
              placeItems: 'center'
            }}>
              {movies.map((movie) => (
                <Card 
                  key={movie.id}
                  onClick={() => handleMovieClick(movie.id)}
                  style={{ cursor: 'pointer' }}
                >
                  <Card.Image 
                     src={`https://image.tmdb.org/t/p/w220_and_h330_face${movie.posterPath}`}
                     srcSet={`
                       https://image.tmdb.org/t/p/w220_and_h330_face${movie.posterPath} 1x,
                       https://image.tmdb.org/t/p/w440_and_h660_face${movie.posterPath} 2x
                     `}
                     alt={movie.title}
                     loading="lazy"
                  />
                  <Card.Body>
                    <Card.Title>{movie.title}</Card.Title>
                  </Card.Body>
                  <Card.Footer>
                    <small>Release Date: {movie.releaseDate}</small>
                  </Card.Footer>
                </Card>
              ))}
            </div>
          </div>
          {totalPages && totalPages > 1 && (
            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={handlePageChange}
            />
          )}
        </>
      )}
    </>
  );
}; 