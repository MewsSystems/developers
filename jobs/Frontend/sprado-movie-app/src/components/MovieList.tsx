import React from "react";
import { Movie } from "../types";
import { MovieCard } from "./MovieCard";

interface MovieListProps {
  movies: Movie[];
  onMovieClick: (movieId: number) => void;
}

export const MovieList = ({ movies, onMovieClick }: MovieListProps) => {
  if (movies.length === 0) {
    return <p className="text-2xl text-center mt-6">No movies found</p>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
      {movies.map((movie) => (
        <div
          key={movie.id}
          className="cursor-pointer"
          onClick={() => onMovieClick(movie.id)}
          onKeyDown={(event) => {
            if (event.key === "Enter") {
              onMovieClick(movie.id);
            }
          }}
          role="button"
          tabIndex={0}
          aria-label={`View details for ${movie.title}`}
        >
          <MovieCard movie={movie} />
        </div>
      ))}
    </div>
  );
};
