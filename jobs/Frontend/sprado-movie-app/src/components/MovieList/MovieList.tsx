import React from "react";
import { Movie } from "../../types";
import { MovieCard } from "../MovieCard/MovieCard";

export const MovieList = ({ movies }: { movies: Movie[] }) => {
  if (movies.length === 0) {
    return <p className="text-2xl text-center mt-6">No movies found</p>;
  }

  return (
    <div
      data-testid="movie-list"
      className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8"
    >
      {movies.map((movie) => (
        <MovieCard movie={movie} key={movie.id} />
      ))}
    </div>
  );
};
