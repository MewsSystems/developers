import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { searchMovies } from './api/movieApi';
import { Movie } from './api/types';


function App() {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState(false);

  const sampleSearch = 'Cars';

  useEffect(() => {
    const fetchInitialMovies = async () => {
      try {
        setLoading(true);
        const response = await searchMovies(sampleSearch);
        setMovies(response.results);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'An error occurred');
      } finally {
        setLoading(false);
      }
    };

    fetchInitialMovies();
  }, []);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <AppContainer>
      <h1>MovieDipslayinator 3000</h1>
      <MovieGrid>
        {movies.map((movie) => (
          <MovieCard key={movie.id}>
            <h3>{movie.title}</h3>
            {movie.poster_path && (
              <MoviePoster
                src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
                alt={movie.title}
              />
            )}
          </MovieCard>
        ))}
      </MovieGrid>
    </AppContainer>
  );
}

export default App;

const AppContainer = styled.div`
  padding: 20px;
`;

const MovieGrid = styled.div`
  display: grid;
  gap: 10px;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
`;

const MovieCard = styled.div`
  border: 1px solid black;
  padding: 10px;
  background-color: #EEEEEE;
  border-radius: 8px;
`;

const MoviePoster = styled.img`
  width: 100%;
  height: auto;
`;
