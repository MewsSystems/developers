import { useGetMoviesSearched } from "@/hooks/useMovies";
import { Card, CardContent, CardTitle } from "../ui/card";
import { useState } from "react";
import { useAppSelector } from "@/hooks/store";
import { useModalState, useMovieId } from "@/hooks/useMoviesActions";

export default function MovieCard() {
  const [page, setPage] = useState<number>(1);
  const moviesSearch = useAppSelector((state) => state.moviesSearch.userSearch);
  const { data, isPlaceholderData } = useGetMoviesSearched(moviesSearch, page);
  const { setModalState } = useModalState();
  const { setMovieId } = useMovieId();

  if (!data || data.length < 1) {
    return <h1></h1>;
  }

  const handleMovieClick = (movieId: number) => {
    setMovieId(movieId);
    setModalState(true);
  };

  return (
    <>
      {data?.map((movie) => (
        <Card key={movie.id}>
          <img
            alt="Movie Poster"
            className="object-cover w-full aspect-[2/3]"
            height={225}
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            width={150}
            onClick={() => handleMovieClick(movie.id)}
          />
          <CardContent>
            <CardTitle className="text-lg font-bold">
              {movie.original_title}
            </CardTitle>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              {}
              Release Year: {movie.release_date.split("-")[0]}
            </p>
            <p className="text-sm">
              Rating: {movie.vote_average.toFixed(1)}/10
            </p>
          </CardContent>
        </Card>
      ))}

      <section>
        <button
          className=" bg-slate-300 text-black border rounded-md px-4 py-1 mr-4 w-36"
          onClick={() => setPage((old) => Math.max(old - 1, 0))}
          disabled={page === 0}
        >
          Previous Page
        </button>
        <button
          className=" bg-slate-300 text-black border rounded-md px-4 py-1 ml-4 w-36"
          onClick={() => {
            if (!isPlaceholderData) {
              setPage((old) => old + 1);
            }
          }}
        >
          Next Page
        </button>
      </section>
    </>
  );
}
