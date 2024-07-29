import React from "react";
import styled from "styled-components";
import { Movie } from "../Form/Form";

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
`;

export const MovieItem:React.FC<MovieItemProps> = ({ movie, onClick }) => {
  return (
    <MovieItemWrapper onClick={onClick}>
      <Poster
        src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
        alt="Movie poster"
      />
      <h2>{movie.title}</h2>
      <p>{movie.overview}</p>
    </MovieItemWrapper>
  );
};
