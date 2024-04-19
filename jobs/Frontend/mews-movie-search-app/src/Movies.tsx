/* Global imports */
import * as React from "react";
import { useMovies } from "./useMovies";
import styled from "styled-components";
import { useLocation } from "wouter";
import { Movie } from "./types/movies";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const Movies = ({ searchTerm }: { searchTerm?: string }) => {
  const { movies } = useMovies({ searchTerm });
  const [, setLocation] = useLocation();

  return movies?.results && movies?.results.length > 0 ? (
    <MovieContainer>
      {movies.results.map((movie: Movie) => {
        return (
          <MovieItem
            key={movie.id}
            onClick={() => setLocation(`/movies/${movie.id}`)}
          >
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
  ) : (
    <NoResults>
      <span>No results are found!</span>
    </NoResults>
  );
};
const MovieContainer = styled.ul`
  display: flex;
  flex-wrap: wrap;
  overflow-y: auto;
  list-style: none;
`;

const MovieItem = styled.li`
  display: flex;
  flex-direction: column;
  list-style: none;
  text-align: center;
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

const NoResults = styled.div`
  padding: 1rem;
  place-content: center;
  height: 100%;
  display: grid;
  & > span {
    font-size: 2rem;
    color: #e3fef7;
  }
`;
