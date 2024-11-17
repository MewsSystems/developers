import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getMovieDetails } from '../api/movieApi';
import { Movie } from '../api';
import styled from 'styled-components';

const TopBar = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: ${({ theme }) => theme.spacing.lg};
`;

const BackButton = styled.button`
  padding: ${({ theme }) => `${theme.spacing.sm} ${theme.spacing.md}`};
  background-color: ${({ theme }) => theme.colors.button.background};
  color: ${({ theme }) => theme.colors.button.text};
  border: none;
  border-radius: ${({ theme }) => theme.borderRadius.md};
  cursor: pointer;
  transition: background-color 0.2s;

  &:hover {
    background-color: ${({ theme }) => theme.colors.button.hover};
  }
`;

const MovieContainer = styled.div`
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: ${({ theme }) => theme.spacing.xl};
  padding: ${({ theme }) => theme.spacing.lg};
  background: ${({ theme }) => theme.colors.surface};
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  
  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const PosterImage = styled.img`
  width: 100%;
  border-radius: ${({ theme }) => theme.borderRadius.md};
  box-shadow: ${({ theme }) => theme.colors.card.shadow};
`;

const MovieInfo = styled.div`
  h2 {
    margin: 0 0 ${({ theme }) => theme.spacing.md} 0;
    font-size: 2rem;
    color: ${({ theme }) => theme.colors.text.primary};
  }

  .rating {
    display: inline-block;
    background: ${({ theme }) => theme.colors.button.background};
    color: ${({ theme }) => theme.colors.button.text};
    padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.md};
    border-radius: ${({ theme }) => theme.borderRadius.md};
    margin-bottom: ${({ theme }) => theme.spacing.md};
  }

  .release-date {
    color: ${({ theme }) => theme.colors.text.secondary};
  }

  .overview {
    line-height: 1.6;
    color: ${({ theme }) => theme.colors.text.primary};
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
      <TopBar>
        <BackButton onClick={() => navigate('/search')}>
          ← Back to Searching
        </BackButton>
      </TopBar>
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
