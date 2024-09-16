import React from "react";
import {
  StyledMovieCardImage,
  StyledMovieCardContainer,
  StyledMovieImgPlaceholder,
} from "./MovieCard.styles";
import { Link } from "react-router-dom";
import type { MovieType } from "../../types";

const MovieCard = ({ movie }: { movie: MovieType }) => {
  return (
    <Link to={`?id=${movie.id}`} data-testid="movie-card">
      <StyledMovieCardContainer>
        {movie.poster_path ? (
          <StyledMovieCardImage
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            alt={movie.title}
          />
        ) : (
          <StyledMovieImgPlaceholder>{movie.title}</StyledMovieImgPlaceholder>
        )}
      </StyledMovieCardContainer>
    </Link>
  );
};

export default MovieCard;
