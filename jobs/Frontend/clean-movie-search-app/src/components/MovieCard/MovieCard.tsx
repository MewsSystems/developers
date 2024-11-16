import React from 'react';
import styled, { css } from 'styled-components';
import { Movie } from '../../api';

const Card = styled.div`
  border-radius: 8px;
  overflow: hidden;
  background: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s;
  cursor: pointer;

  &:hover {
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
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
  padding: 16px;
`;

const Title = styled.h3`
  margin: 0 0 8px 0;
  font-size: 1rem;
  font-weight: 600;
`;

const ReleaseDate = styled.p`
  margin: 0;
  color: #666;
  font-size: 0.875rem;
`;

const Rating = styled.div<{ rating: number }>`
  position: absolute;
  top: 8px;
  left: 8px;
  background: rgba(0, 0, 0, 0.75);
  color: white;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 0.875rem;

  ${({ rating }) =>
    rating >= 7.5 &&
    css`
      color: gold;
      border: 2px solid gold;
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
    <Card onClick={() => onClick(movie.id)}>
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
        <Rating rating={movie.vote_average}>{movie.vote_average.toFixed(1)}</Rating>
      </PosterContainer>
      <Content>
        <Title>{movie.title}</Title>
        {isValidDate && (
          <ReleaseDate>
            {releaseDate.toLocaleDateString()}
          </ReleaseDate>
        )}
      </Content>
    </Card>
  )
};