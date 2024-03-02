import React from "react";
import Link from "next/link";
import { MovieDetail as MovieDetailType } from "@/scenes/MovieDetail/services/types";
import { Badge } from "@/components/ui/badge";
import MovieDetailContent from "@/scenes/MovieDetail/components/MovieDetailContent";
import { Button } from "@/components/ui/button";

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
      className="w-screen h-screen bg-center bg-cover bg-no-repeat"
    >
      <div className="absolute top-0 left-0 w-screen h-screen bg-gray-700/80 z-0"></div>
      <div className="z-10 text-white relative h-screen">
        <Link href="/" className="absolute top-5 left-5">
          <Button variant="link" className="text-gray-300">
            Back to search
          </Button>
        </Link>
        <div className="flex flex-row h-full">
          <div className="w-1/2 pl-12 justify-center flex flex-col gap-3">
            <MovieDetailContent movie={movie} />
          </div>
          <div className="w-1/2 flex flex-row justify-center items-center">
            <img
              src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
              alt={movie.title}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default MovieDetail;
