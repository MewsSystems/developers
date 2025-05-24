import { useNavigate, useSearchParams } from 'react-router-dom';
import { getMovies, searchMovies } from "@movie/services/api/movie-api";
import { useService } from "@app/lib/useService";
import { Card } from "@app/lib/components/card/card";
import { Search } from "@app/lib/components/search/search";
import { Pagination } from "@app/lib/components/pagination/pagination";
import { useState } from 'react';
import { MovieCardsSkeleton } from '@app/lib/components/skeleton-cards-list/skeleton-cards-list';
import { ErrorComponent } from '@core/error/components/error-component';

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

  return (
    <>
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
              gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
              gap: '2rem'
            }}>
              {movies.map((movie) => (
                <Card 
                  key={movie.id}
                  onClick={() => handleMovieClick(movie.id)}
                  style={{ cursor: 'pointer' }}
                >
                  <Card.Image 
                    src={`https://image.tmdb.org/t/p/w500${movie.posterPath}`} 
                    alt={movie.title}
                  />
                  <Card.Body>
                    <Card.Title>{movie.title}</Card.Title>
                    <Card.Description>{movie.overview}</Card.Description>
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