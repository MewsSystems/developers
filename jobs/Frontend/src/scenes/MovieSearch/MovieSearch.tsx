"use client";

import React, { useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { MovieSearchResult } from "@/scenes/MovieSearch/services/types";
import fetchMovies from "@/scenes/MovieSearch/services/fetchMovies";
import MovieCard from "@/scenes/MovieSearch/components/MovieCard";

const MovieSearch = () => {
  const [moviesData, setMoviesData] = useState<MovieSearchResult | null>(null);
  const [query, setQuery] = useState<string | null>(null);
  const onInputChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.value) {
      setQuery(null);
      setMoviesData(null);
      return;
    }
    setQuery(e.target.value);
    const movies = await fetchMovies(e.target.value);
    setMoviesData(movies);
  };
  return (
    <div className="flex flex-col items-center gap-4">
      <h1>Movie Search</h1>
      <Input
        className="w-[300px]"
        type="text"
        placeholder="Search for a movie..."
        onChange={onInputChange}
      />
      {moviesData && (
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          {moviesData.results.map((movie) => (
            <MovieCard key={movie.id} movie={movie} />
          ))}
        </div>
      )}
    </div>
  );
};

export default MovieSearch;
