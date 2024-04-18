/* Global imports */
import * as React from "react";
import { Movie, useMovies } from "./useMovies";
import styled from "styled-components";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const Movies = () => {
  const { movies } = useMovies();
  return (
    <MovieContainer>
      {movies?.results &&
        movies?.results.map((movie: Movie) => {
          return (
            <MovieItem key={movie.id}>
              <MoviePoster>
                <MoviePosterImage
                  src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
                  alt={movie.title}
                />
              </MoviePoster>
            </MovieItem>
          );
        })}
    </MovieContainer>
  );
};
const MovieContainer = styled.ul`
  display: flex;
  flex-wrap: wrap;
  overflow-y: auto;
`;

const MovieItem = styled.li`
  display: flex;
  flex-wrap: wrap;
  overflow-y: auto;
`;
const MoviePosterImage = styled.img`
  width: 100%;
  height: 100%;
  border-width: 8px;
  border-radius: 1.5rem;
`;
const MoviePoster = styled.div`
  padding: 1rem;
`;
