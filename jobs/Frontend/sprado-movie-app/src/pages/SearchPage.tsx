import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { SearchBar } from "../components/SearchBar";
import { MovieList } from "../components/MovieList";
import { useMoviesFetch } from "../hooks/useMoviesFetch";
import { Movie } from "../types";

export const SearchPage: React.FC = () => {
  const [search, setSearch] = useState("");
  const {
    data: movies,
    isLoading,
    error,
  } = useMoviesFetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${process.env.REACT_APP_TMDB_API_KEY}&language=en-US&query=${
      search
    }&page=1&include_adult=false`
  );

  const navigate = useNavigate();

  const handleSearchChange = (query: string) => {
    setSearch(query);
  };

  const handleMovieClick = (movieId: number) => {
    navigate(`/movie/${movieId}`);
  };

  return (
    <div className="bg-darkDefault min-h-screen py-12">
      <div className="max-w-5xl mx-auto px-6 mb-8">
        <SearchBar value={search} onChange={handleSearchChange} />
      </div>

      <div className="max-w-7xl mx-auto px-6">
        {isLoading && <p className="text-center text-gray-400">Loading...</p>}
        {error && <p className="text-center text-red-500">{error.message}</p>}

        {!isLoading && !error && search.trim() && movies.length === 0 && (
          <p className="text-center text-gray-400">No movies found.</p>
        )}

        {!isLoading && !error && movies.length > 0 && (
          <MovieList movies={movies} onMovieClick={handleMovieClick} />
        )}
      </div>
    </div>
  );
};
