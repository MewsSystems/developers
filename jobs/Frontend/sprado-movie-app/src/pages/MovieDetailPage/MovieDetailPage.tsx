import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Movie } from "../../types";
import FallbackPoster from "../../assets/fallback-poster.png";
import { useFetchMovieDetails } from "../../hooks/useFetchMovieDetails/useFetchMovieDetails";

export const MovieDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const { movie, isLoading, error } = useFetchMovieDetails(id);
  const navigate = useNavigate();

  if (isLoading)
    return <p className="text-center text-gray-300 mt-6">Loading...</p>;
  if (error)
    return <p className="text-center text-red-500 mt-6">{error.message}</p>;
  if (!movie)
    return (
      <p className="text-center text-gray-300 mt-6">No movie details found.</p>
    );

  const backdropUrl = movie.backdrop_path
    ? `https://image.tmdb.org/t/p/original${movie.backdrop_path}`
    : null;

  return (
    <div
      className="relative flex justify-center items-center min-h-screen py-20 px-4 bg-gradient-to-b from-black via-gray-900 to-black"
      style={{
        backgroundImage: backdropUrl ? `url(${backdropUrl})` : undefined,
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        backgroundColor: backdropUrl ? undefined : "black",
      }}
    >
      <button
        className="absolute top-8 left-8 text-white text-2xl bg-black bg-opacity-80 rounded-full px-4 py-2 hover:bg-opacity-75 focus:outline-none"
        onClick={() => navigate(-1)}
        aria-label="Go back"
      >
        &larr; Back
      </button>

      <div className="backdrop-blur-sm bg-black bg-opacity-80 rounded-lg min-h-full mt-6 flex items-center justify-center w-full">
        <div className="max-w-5xl mx-auto px-6 py-10 text-white">
          <div className="flex flex-col md:flex-row items-center md:items-start md:justify-center gap-32">
            <div className="w-full md:w-1/2 flex justify-center md:justify-end">
              <img
                src={
                  movie.poster_path
                    ? `https://image.tmdb.org/t/p/w500${movie.poster_path}`
                    : FallbackPoster
                }
                alt={movie.title}
                className="rounded-lg shadow-2xl max-w-[300px] md:max-w-full"
              />
            </div>

            <div className="w-full md:w-1/2 text-center md:text-left flex flex-col items-center md:items-start">
              <h2 className="text-6xl font-bold mb-6 drop-shadow-lg">
                {movie.title}
              </h2>
              <p className="text-2xl text-gray-300 mb-6">
                <span className="font-semibold">Release Date:</span>{" "}
                {movie.release_date || "N/A"}
              </p>
              <p className="text-xl text-gray-200 leading-relaxed mb-8 max-w-3xl">
                {movie.overview}
              </p>
              <p className="text-2xl text-gray-200 font-semibold">
                Rating:{" "}
                {movie.vote_average !== undefined && movie.vote_average !== null
                  ? movie.vote_average.toFixed(2)
                  : "N/A"}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
