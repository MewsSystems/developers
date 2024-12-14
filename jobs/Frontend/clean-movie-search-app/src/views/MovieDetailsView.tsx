import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getMovieDetails } from '../api/movieApi';
import { Movie } from '../api';
import styled from 'styled-components';

const TopBar = styled.div.attrs({
  className: 'top-bar'
})`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: ${({ theme }) => theme.spacing.lg};
`;

const BackButton = styled.button.attrs({
  className: 'back-button'
})`
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

const MovieContainer = styled.div.attrs({
  className: 'movie-container'
})`
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

const PosterImage = styled.img.attrs({
  className: 'poster-image'
})`
  width: 100%;
  border-radius: ${({ theme }) => theme.borderRadius.md};
  box-shadow: ${({ theme }) => theme.colors.card.shadow};
`;

const MovieInfo = styled.div.attrs({
  className: 'movie-info'
})`
  display: flex;
  flex-direction: column;
`;

const MovieTitle = styled.h2`
  margin: 0 0 ${({ theme }) => theme.spacing.xs} 0;
  font-size: 2rem;
  color: ${({ theme }) => theme.colors.text.primary};
`;

const Rating = styled.div`
  display: inline-block;
  background: ${({ theme }) => theme.colors.button.background};
  color: ${({ theme }) => theme.colors.button.text};
  padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.md};
  border-radius: ${({ theme }) => theme.borderRadius.md};
  margin-bottom: ${({ theme }) => theme.spacing.md};
  width: fit-content;
`;

const ReleaseDate = styled.div`
  color: ${({ theme }) => theme.colors.text.secondary};
`;

const Overview = styled.p`
  line-height: 1.6;
  color: ${({ theme }) => theme.colors.text.primary};
`;

export const MovieDetailsView: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  // State for the movie details page, not in context because it's not to be "cached"
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    const fetchMovie = async () => {
      if (!id) {
        setError('Hey there! Yout are not looking for any particular movie :(');
        setLoading(false)
        return;
      }

      const movieId = parseInt(id, 10);
      if (isNaN(movieId)) {
        setError('Hey there! You are looking for an invalid movie ID.');
        setLoading(false)
        return;
      }

      try {
        setLoading(true);
        const movieData = await getMovieDetails(movieId);
        setMovie(movieData);
      } catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : 'Sorry, an error occurred while retrieving the movie details'
        );
      } finally {
        setLoading(false);
      }
    };

    fetchMovie();
  }, [id]);

  // TODO: better error handling
  if (loading) return <div>Loading movie details...</div>;
  if (error) return <div>{error}</div>;
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
            // Fallback image in case the movie doesn't have a poster
            src={`https://via.placeholder.com/500x750?text=${encodeURIComponent(
              movie.title
            )}`}
            alt={movie.title}
          />
        )}
        <MovieInfo>
          <MovieTitle>{movie.title}</MovieTitle>
          <Rating>Rating: {movie.vote_average.toFixed(1)}</Rating>
          {isValidDate && (
            <ReleaseDate>
              Released: {new Date(movie.release_date).toLocaleDateString()}
            </ReleaseDate>
          )}
          <Overview>{movie.overview}</Overview>
        </MovieInfo>
      </MovieContainer>
    </>
  );
};
