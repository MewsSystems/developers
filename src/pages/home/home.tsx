import React from 'react';
import { useNavigate } from 'react-router-dom';
import { getMovies, searchMovies } from "@movie/services/api/movie-api";
import { useService } from "@app/lib/useService";
import { Card } from "@app/lib/components/card/card";
import { Search } from "@app/lib/components/search/search";

export const Home: React.FC = () => {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = React.useState('');
  const { data: popularMovies, loading: popularLoading, error: popularError } = useService(getMovies);
  const { data: searchResults, loading: searchLoading, error: searchError } = useService(
    () => searchQuery.trim() ? searchMovies(searchQuery) : Promise.resolve([]),
    { dependencies: [searchQuery] }
  );

  const movies = searchQuery.trim() ? searchResults : popularMovies;
  const loading = searchQuery.trim() ? searchLoading : popularLoading;
  const error = searchQuery.trim() ? searchError : popularError;

  const handleMovieClick = (movieId: number) => {
    navigate(`/movie/${movieId}`);
  };

  return (
    <>
      <Search onSearch={setSearchQuery} />
      {error ? (
        <div>Error: {error}</div>
      ) : loading ? (
        <div>Loading...</div>
      ) : !movies?.length ? (
        <h3>No movies found</h3>
      ) : (
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
      )}
    </>
  );
}; 