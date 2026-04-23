"use client";

import React, { useEffect, useState } from "react";
import { MovieSearchResult } from "@/scenes/MovieSearch/services/types";
import fetchMovies from "@/scenes/MovieSearch/services/fetchMovies";
import MovieCard from "@/scenes/MovieSearch/components/MovieCard";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { Loader2 } from "lucide-react";
import MoviesPagination from "@/scenes/MovieSearch/components/MoviesPagination";
import DebouncedInput from "@/scenes/MovieSearch/components/DebouncedInput";

/**
 * The client-side MovieSearch scene which is used to search for movies using the TMDB API. It displays a search input and the results
 * in a grid. The user can navigate through the results using the pagination component. To save the query for the search
 * and the current page, the search params in URL are used. The search input is debounced to avoid making too many requests.
 */
const MovieSearch = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [moviesData, setMoviesData] = useState<MovieSearchResult | null>(null);
  const searchParams = useSearchParams();
  const query = searchParams.get("query");
  const page = Number(searchParams.get("page") ?? "1");
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    setError(null);
    if (query && !isNaN(page)) {
      setIsLoading(true);
      fetchMovies(query, page)
        .then((data) => {
          setMoviesData(data);
          setIsLoading(false);
        })
        .catch(() => {
          setMoviesData(null);
          setIsLoading(false);
          setError(
            "An error occurred while fetching the data. Please try again later.",
          );
        });
    } else {
      setMoviesData(null);
      setIsLoading(false);
    }
  }, [query, page]);

  const onInputChange = (value: string) => {
    const urlParams = new URLSearchParams(searchParams.toString());
    urlParams.delete("page");
    if (value) {
      urlParams.set("query", value);
    } else {
      urlParams.delete("query");
    }
    router.push(`${pathname}?${urlParams.toString()}`);
  };
  return (
    <div className="flex flex-col items-center pt-16 md:pt-32 gap-8 px-5 pb-32">
      <h1 className="text-5xl font-bold">Movie Search</h1>
      <DebouncedInput
        className="w-full md:w-1/2 lg:w-1/3 xl:w-1/4"
        type="text"
        placeholder="Search for a movie..."
        debouncedOnChange={onInputChange}
        initialValue={query ?? undefined}
        onChange={() => {
          setIsLoading(true);
        }}
      />
      {isLoading && !moviesData && (
        <Loader2 className="mr-2 h-4 w-4 animate-spin" />
      )}
      {error && !isLoading && <p className="text-red-500">{error}</p>}
      {moviesData && (
        <div className="w-full md:w-fit">
          {moviesData.results.length === 0 ? (
            <p className="text-center text-lg">No movies found</p>
          ) : (
            <div>
              <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-4">
                {moviesData.results.map((movie) => (
                  <MovieCard key={movie.id} movie={movie} />
                ))}
              </div>
              <MoviesPagination
                totalPages={moviesData.total_pages}
                currentPage={page}
              />
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default MovieSearch;
