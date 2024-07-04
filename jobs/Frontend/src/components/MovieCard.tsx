import React from "react";
import styled from "styled-components";
import StyledLink from "./StyledLink";
import type { Movie } from "../api";

const Card = styled.div`
  padding: 8px;
  border-radius: 8px;
  flex: 0 1;
  width: 200px;
`;

const RoundedImg = styled.img`
  border-radius: 8px;
  width: 130px;
`;

const RoundedImgPlaceholder = styled.div`
  border-radius: 8px;
  width: 130px;
  height: 195px;
  background-color: #f0f0f0;
`;

const MovieTitle = styled.p`
  margin: 0;
  text-align: center;
  font-size: 0.9em;
`;

const MovieCard: React.FC<{ movie: Movie }> = ({ movie }) => {
  return (
    <Card>
      <StyledLink to={`/movie/${movie.id}`}>
        {movie.poster_path ? (
          <RoundedImg
            src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
            alt={movie.title}
          />
        ) : (
          <RoundedImgPlaceholder />
        )}
        <MovieTitle>{movie.title}</MovieTitle>
      </StyledLink>
    </Card>
  );
};

export default MovieCard;
