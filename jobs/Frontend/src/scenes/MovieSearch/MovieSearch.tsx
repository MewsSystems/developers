"use client";

import React, { useCallback, useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { MovieSearchResult } from "@/scenes/MovieSearch/services/types";
import fetchMovies from "@/scenes/MovieSearch/services/fetchMovies";
import MovieCard from "@/scenes/MovieSearch/components/MovieCard";
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { Loader2 } from "lucide-react";
import MoviesPagination from "@/scenes/MovieSearch/components/MoviesPagination";
import DebouncedInput from "@/scenes/MovieSearch/components/DebouncedInput";

const MovieSearch = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [moviesData, setMoviesData] = useState<MovieSearchResult | null>(null);
  const searchParams = useSearchParams();
  const query = searchParams.get("query");
  const page = Number(searchParams.get("page") ?? "1");
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    if (query && !isNaN(page)) {
      fetchMovies(query, page).then((data) => {
        setMoviesData(data);
        setIsLoading(false);
      });
    } else {
      setMoviesData(null);
      setIsLoading(false);
    }
  }, [query, page]);

  const onInputChange = (value: string) => {
    const urlParams = new URLSearchParams(searchParams.toString());
    if (!value) {
      urlParams.delete("query");
      urlParams.delete("page");
      router.push(`${pathname}?${urlParams.toString()}`);
      return;
    }
    urlParams.set("query", value);
    urlParams.delete("page");
    router.push(`${pathname}?${urlParams.toString()}`);
  };
  return (
    <div className="flex flex-col items-center pt-32 gap-8 px-5 pb-32">
      <h1 className="text-5xl font-bold">Movie Search</h1>
      <DebouncedInput
        className="w-full md:w-1/3"
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
      {moviesData && (
        <>
          {moviesData.results.length === 0 ? (
            <p className="text-center text-lg">No movies found</p>
          ) : (
            <div>
              <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
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
        </>
      )}
    </div>
  );
};

export default MovieSearch;
