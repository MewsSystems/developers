import React from "react";
import FallbackPoster from "../../assets/fallback-poster.png";

import type { Movie } from "../../types";
import { useNavigate } from "react-router-dom";

export const MovieCard = ({ movie }: { movie: Movie }) => {
  const navigate = useNavigate();

  const handleMovieClick = () => {
    navigate(`/movie/${movie.id}`);
  };

  return (
    <div
      className="bg-darkSoft rounded-lg overflow-hidden shadow-lg transition-transform duration-200 hover:scale-105 hover:shadow-2xl flex flex-col h-full cursor-pointer"
      onClick={handleMovieClick}
      onKeyDown={(event) => {
        if (event.key === "Enter") {
          handleMovieClick();
        }
      }}
      role="button"
      tabIndex={0}
      aria-label={`View details for ${movie.title}`}
    >
      <img
        src={
          movie.poster_path
            ? `https://image.tmdb.org/t/p/w500${movie.poster_path}`
            : FallbackPoster
        }
        loading="lazy"
        alt={movie.title}
        className="w-full h-56 object-cover"
      />
      <div className="p-4">
        <h2 className="text-lg font-semibold text-white mb-2 line-clamp-2">
          {movie.title}
        </h2>
        <p className="text-gray-400">
          Rating:{" "}
          {movie.vote_average !== undefined && movie.vote_average !== null
            ? movie.vote_average.toFixed(2)
            : "N/A"}
        </p>
      </div>
    </div>
  );
};
