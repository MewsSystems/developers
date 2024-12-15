import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { SearchBar } from "../components/SearchBar";
import { MovieList } from "../components/MovieList";
import { useMoviesFetch } from "../hooks/useMoviesFetch";
import { Movie } from "../types";
import { Pagination } from "../components/Pagination";

export const SearchPage = () => {
  const [search, setSearch] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const {
    data: movies,
    totalPages,
    isLoading,
    error,
  } = useMoviesFetch(
    `https://api.themoviedb.org/3/search/movie?api_key=${process.env.REACT_APP_TMDB_API_KEY}&language=en-US&include_adult=false`,
    search,
    currentPage
  );

  const navigate = useNavigate();

  const handleSearchChange = (query: string) => {
    setSearch(query);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handleMovieClick = (movieId: number) => {
    navigate(`/movie/${movieId}`);
  };

  return (
    <div className="bg-darkDefault min-h-screen py-12">
      <div className="text-center mb-12">
        <h1 className="text-6xl font-extrabold font-title text-purple-600 leading-tight drop-shadow-lg">
          Cinematic 🎬
        </h1>
        <p className="text-lg text-gray-300 mt-6 max-w-3xl mx-auto leading-relaxed">
          Your gateway to a world of movies and series. Stream, explore, and
          discover the stories that inspire you, all in one place.
        </p>
      </div>
      <div className="max-w-5xl mx-auto px-6 mb-8">
        <SearchBar value={search} onChange={handleSearchChange} />
      </div>

      <div className="max-w-[80%] mx-auto px-6">
        {isLoading && <p className="text-center text-gray-400">Loading...</p>}
        {error && <p className="text-center text-red-500">{error.message}</p>}

        {!isLoading && !error && search.trim() && movies.length === 0 && (
          <p className="text-center text-gray-400">No movies found.</p>
        )}

        {!isLoading && !error && movies.length > 0 && (
          <>
            <MovieList movies={movies} onMovieClick={handleMovieClick} />
            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={handlePageChange}
            />
          </>
        )}
      </div>
    </div>
  );
};
