import React from 'react';
import styled, { css } from 'styled-components';
import { Movie } from '../../api';

const Card = styled.article`
  border-radius: ${({ theme }) => theme.borderRadius.md};
  overflow: hidden;
  background: ${({ theme }) => theme.colors.card.background};
  box-shadow: ${({ theme }) => theme.colors.card.shadow};
  transition:
    transform 0.2s,
    box-shadow 0.2s,
    background-color 0.3s;
  cursor: pointer;

  &:hover {
    transform: translateY(-2px);
    box-shadow: ${({ theme }) => theme.colors.card.hoverShadow};
  }
`;

const PosterContainer = styled.div`
  position: relative;
  padding-top: 150%;
`;

const Poster = styled.img`
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
`;

const Content = styled.div`
  padding: ${({ theme }) => theme.spacing.md};
`;

const Title = styled.h3`
  margin: 0 0 ${({ theme }) => theme.spacing.sm} 0;
  font-size: 1rem;
  font-weight: 600;
  color: ${({ theme }) => theme.colors.text.primary};
`;

const ReleaseDate = styled.p`
  margin: 0;
  color: ${({ theme }) => theme.colors.text.secondary};
  font-size: 0.875rem;
`;

const Rating = styled.div<{ rating: number }>`
  position: absolute;
  top: ${({ theme }) => theme.spacing.sm};
  left: ${({ theme }) => theme.spacing.sm};
  background: ${({ theme }) => theme.colors.rating.background};
  color: ${({ theme }) => theme.colors.rating.text};
  padding: ${({ theme }) => `${theme.spacing.xs} ${theme.spacing.sm}`};
  border-radius: ${({ theme }) => theme.borderRadius.sm};
  font-size: 0.875rem;

  ${({ rating, theme }) =>
    rating >= 7.5 &&
    css`
      color: ${theme.colors.rating.highRating.text};
      border: 2px solid ${theme.colors.rating.highRating.border};
    `}
`;

interface MovieCardProps {
  movie: Movie;
  onClick: (id: number) => void;
}

export const MovieCard: React.FC<MovieCardProps> = ({ movie, onClick }) => {
  const releaseDate = new Date(movie.release_date);
  const isValidDate = !isNaN(releaseDate.getDate());

  return (
    <Card onClick={() => onClick(movie.id)} aria-label={`Movie card for ${movie.title}`}> 
      <PosterContainer>
        {movie.poster_path ? (
          <Poster
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            alt={movie.title}
            loading="lazy"
          />
        ) : (
          <Poster
            src={`https://via.placeholder.com/500x750?text=${encodeURIComponent(
              movie.title
            )}`}
            alt={movie.title}
          />
        )}
        {typeof movie.vote_average === 'number' && ( 
          <Rating rating={movie.vote_average} aria-label={`Rating: ${movie.vote_average.toFixed(1)}`}> 
            {movie.vote_average.toFixed(1)}
          </Rating>
        )}
      </PosterContainer>
      <Content>
        <Title>{movie.title}</Title>
        {isValidDate && (
          <ReleaseDate>{releaseDate.toLocaleDateString()}</ReleaseDate>
        )}
      </Content>
    </Card>
  );
};