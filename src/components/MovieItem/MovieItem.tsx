import React from "react";
import styled from "styled-components";
import { Movie } from "../Form/Form";
import MissingPoster from "../Form/MissingPoster.jpg"

interface MovieItemProps {
  movie: Movie;
  onClick: () => void;
}

const MovieItemWrapper = styled.li`
  margin: 20px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 25%;
  font-size: 2vmin;
`;

const Poster = styled.img`
  height: 30vmin;
  max-width: 100%;
`;

export const MovieItem:React.FC<MovieItemProps> = ({ movie, onClick }) => {
  return (
    <MovieItemWrapper onClick={onClick}>
      <Poster
        src={movie.poster_path? `https://image.tmdb.org/t/p/w500/${movie.poster_path}`:MissingPoster}
        alt="Movie poster"
      />
      <h2>{movie.title}</h2>
      <p>{movie.overview}</p>
    </MovieItemWrapper>
  );
};
