import React from "react";
import styled, { css } from "styled-components";
import { useNavigate } from "react-router-dom";
import { DetailedMovie, Movie } from "../../../models/tmdbModels";

export const MoviePosterCard: React.FC<{
  movie: Movie | DetailedMovie;
  info?: boolean;
}> = ({ movie, info }) => {
  const nav = useNavigate();

  return (
    <Container info={info} onClick={() => nav(`movie/${movie.id}`)}>
      <PosterImage
        src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
        alt={movie.title}
        info={info}
      />
      {info && (
        <InfoOverlay>
          <Title>{movie.title}</Title>
          <Details>
            <p>
              <span>Language:</span> {movie.original_language}
            </p>
            <p>
              <span>Release:</span> {movie.release_date}
            </p>
            <p>
              <span>Popularity:</span> {movie.popularity.toFixed(1)}
            </p>
            <p>
              <span>Vote Average:</span> {movie.vote_average}/10
            </p>
          </Details>
          <Overview>{movie.overview}</Overview>
        </InfoOverlay>
      )}
    </Container>
  );
};

const Container = styled.div<{ info?: boolean }>`
  position: relative;
  width: 9rem;
  height: 100%;
  border-radius: 0.5rem;
  overflow: hidden;
  background-color: #2d3748;
  box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);

  ${({ info }) =>
    info &&
    css`
      cursor: pointer;
    `}

  @media (min-width: 768px) {
    width: 100%;
  }
`;

const PosterImage = styled.img<{ info?: boolean }>`
  width: 100%;
  height: 100%;
  ${({ info }) =>
    info &&
    css`
      object-cover: cover;
      transition: transform 0.3s;
      &:hover {
        transform: scale(1.05);
      }
    `}
`;

const InfoOverlay = styled.div`
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.8);
  opacity: 0;
  transition: opacity 0.3s;
  padding: 1rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  &:hover {
    opacity: 1;
  }
`;

const Title = styled.h3`
  color: white;
  font-size: 1.125rem;
  font-weight: 600;
`;

const Details = styled.div`
  color: #d1d5db;
  font-size: 0.875rem;
  span {
    font-weight: bold;
  }
`;

const Overview = styled.p`
  color: #9ca3af;
  font-size: 0.75rem;
  margin-top: 0.5rem;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
`;
