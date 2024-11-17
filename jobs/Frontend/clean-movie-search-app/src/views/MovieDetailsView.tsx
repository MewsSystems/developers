import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getMovieDetails } from '../api/movieApi';
import { Movie } from '../api';
import styled from 'styled-components';

const BackButton = styled.button`
  margin-bottom: 20px;
  padding: 12px 12px;
  background-color: #0277bd;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;

  &:hover {
    background-color: #01579b;
  }
`;

const MovieContainer = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 24px;
  padding: 20px;
  background: white;
  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const PosterImage = styled.img`
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
`;

const MovieInfo = styled.div`
  h2 {
    margin: 0 0 16px 0;
    font-size: 2rem;
  }

  .rating {
    display: inline-block;
    background: #0277bd;
    color: white;
    padding: 4px 8px;
    border-radius: 8px;
    margin-bottom: 16px;
    padding: 8px 8px;
  }

  .release-date {
    color: #757575;
  }
  .overview {
    line-height: 1.6;
  }
`;

export const MovieDetailsView: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    const fetchMovie = async () => {
      if (!id) return;

      try {
        setLoading(true);
        const movieData = await getMovieDetails(parseInt(id, 10));
        setMovie(movieData);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : 'Sorry, an error occurred while retrieving the movie detail :('
        );
      } finally {
        setLoading(false);
      }
    };

    fetchMovie();
  }, [id]);

  if (loading) return <div>Loading movie details...</div>;
  if (error) return <div>Error: {error}</div>;
  if (!movie) return <div>Movie not found</div>;

  const releaseDate = new Date(movie.release_date);
  const isValidDate = !isNaN(releaseDate.getDate());

  return (
    <>
      <BackButton onClick={() => navigate('/search')}>
        ← Back to Searching
      </BackButton>
      <MovieContainer>
        {movie.poster_path ? (
          <PosterImage
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            alt={movie.title}
          />
        ) : (
          <PosterImage
            src={`https://via.placeholder.com/500x750?text=${encodeURIComponent(
              movie.title
            )}`}
            alt={movie.title}
          />
        )}
        <MovieInfo>
          <h2>{movie.title}</h2>
          <div className="rating">Rating: {movie.vote_average.toFixed(1)}</div>
          {isValidDate && (
            <div className="release-date">
              Released: {new Date(movie.release_date).toLocaleDateString()}
            </div>
          )}
          <p className="overview">{movie.overview}</p>
        </MovieInfo>
      </MovieContainer>
    </>
  );
};
