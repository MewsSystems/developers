import React from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";
import type { Movie } from "../api";

const Card = styled.div`
  padding: 16px;
  margin: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
`;

const MovieCard: React.FC<{ movie: Movie }> = ({ movie }) => {
  return (
    <Card>
      <Link to={`/movie/${movie.id}`}>
        <h2>{movie.title}</h2>
        <img
          src={`https://image.tmdb.org/t/p/w200${movie.poster_path}`}
          alt={movie.title}
        />
      </Link>
    </Card>
  );
};

export default MovieCard;
