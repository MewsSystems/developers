import React from "react";
import styled from "styled-components";
import MovieCard from "./MovieCard";
import type { Movie } from "../api";

const Grid = styled.div`
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
`;

const MovieCardGrid: React.FC<{ movies: Movie[] }> = ({ movies }) => {
    console.log(movies);
  return (
    <Grid>
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </Grid>
  );
};

export default MovieCardGrid;
