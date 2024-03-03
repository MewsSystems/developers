import React from "react";
import { MovieDetail as MovieDetailType } from "@/scenes/MovieDetail/services/types";
import MovieDetailContent from "@/scenes/MovieDetail/components/MovieDetailContent";
import BackToSearchLink from "@/scenes/MovieDetail/components/BackToSearchLink";

type MovieDetailProps = {
  movieId: string;
};
const MovieDetail = async ({ movieId }: MovieDetailProps) => {
  const res = await fetch(
    `https://api.themoviedb.org/3/movie/${movieId}?api_key=${process.env.NEXT_PUBLIC_MOVIES_API_KEY}`,
  );
  const movie = (await res.json()) as MovieDetailType;
  return (
    <div
      style={{
        backgroundImage: `url(https://image.tmdb.org/t/p/original${movie.backdrop_path})`,
      }}
      className="w-screen h-screen bg-center bg-cover bg-no-repeat overflow-y-scroll"
    >
      {movie.backdrop_path && (
        <div className="absolute top-0 left-0 w-screen h-screen bg-gray-700/80 z-0"></div>
      )}
      <div className="z-10 text-white relative md:h-screen">
        <BackToSearchLink />
        <div className="flex flex-row h-full">
          <div className="w-full md:w-2/3 xl:w-1/2 p-5 md:pl-12 justify-center flex flex-col gap-3">
            <MovieDetailContent movie={movie} />
          </div>
          <div className="md:w-1/3 pr-5 xl:pr-0 xl:w-1/2 flex-row justify-center items-center hidden md:flex">
            {movie.poster_path && (
              <img
                src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                alt={movie.title}
              />
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default MovieDetail;
