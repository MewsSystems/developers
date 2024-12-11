import React from "react";
import { Movie } from "@/scenes/MovieSearch/services/types";
import Link from "next/link";

type MovieCardProps = {
  movie: Movie;
};

/**
 * Component used in the MovieSearch scene to display a movie card in the results. It shows the movie title,
 * release year and has the poster as a background image if available.
 *
 * @param movie - The movie to display in the card.
 */
const MovieCard = ({ movie }: MovieCardProps) => {
  const hasPoster = Boolean(movie.poster_path);
  return (
    <Link href={`/detail/${movie.id}`}>
      <div
        className={`w-full md:w-[200px] h-[200px] flex flex-col items-center px-1 justify-center border-2 relative overflow-hidden ${hasPoster ? "before:absolute before:inset-0 before:bg-gray-800 before:opacity-75 before:content-[''] before:z-10" : ""}`}
      >
        <h2 className="z-20 relative text-white text-center font-semibold">
          {movie.title}
        </h2>
        <p className="z-20 relative text-white">
          {movie.release_date?.split("-")[0]}
        </p>
        {movie.poster_path && (
          <img
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            alt={movie.title}
            className="w-full h-full object-cover absolute"
          />
        )}
      </div>
    </Link>
  );
};

export default MovieCard;
