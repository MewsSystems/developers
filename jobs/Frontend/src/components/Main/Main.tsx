import { useEffect, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { SearchIcon } from "lucide-react";

import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { SearchResults } from "../Search/Search";
import { useDebounce } from "src/lib/utils";
import useMovies from "src/hooks/useMovies";
import useMovieDetails from "src/hooks/useMovieDetails";
import { MovieDetails } from "../Movies/MovieDetails";
import { useQueryClient } from "@tanstack/react-query";

export const Main = () => {
  const [searchParams, setSearchParams] = useSearchParams();

  const initialQuery = searchParams.get("search") || "";
  const [search, setSearch] = useState(initialQuery);

  const { movieId } = useParams<{ movieId: string }>();

  const debouncedSearch = useDebounce(search, 500);

  const movies = useMovies(
    searchParams.get("search") ?? debouncedSearch,
    searchParams.get("page") ?? "1"
  );
  const movieDetails = useMovieDetails(movieId!);

  const navigate = useNavigate();

  useEffect(() => {
    if (debouncedSearch) {
      setSearchParams({
        search: debouncedSearch,
        page: "1",
      });
    } else {
      setSearchParams({});
    }
  }, [debouncedSearch]);

  useEffect(() => {
    setSearch(searchParams.get("search")!);
  }, [setSearchParams]);

  return (
    <div className="flex min-h-screen bg-gradient-to-b from-lime-300 to-lime-500">
      <section className="w-full pt-24 md:pt-24">
        <div className="container px-4 md:px-6">
          <div className="flex h-full w-full items-center">
            <div className="relative w-full pr-4">
              <Input
                placeholder="Search for movies"
                onChange={(e) => setSearch(e.target.value)}
                className="h-full rounded-sm !text-lg"
                value={search ?? ""}
              />
              <SearchIcon className="absolute right-8 top-2" />
            </div>
            <Button
              className="h-full w-48 rounded-sm"
              onClick={() => {
                navigate("/", { replace: false });
              }}
            >
              Clear search
            </Button>
          </div>
          <div className="mt-4 grid grid-cols-12 gap-6">
            <div className="col-span-7 flex flex-col justify-center text-center">
              <div className="mb-2">
                <SearchResults movies={movies} />
              </div>
            </div>
            <div className="col-span-5 flex" aria-label="Movie details panel">
              <MovieDetails movieDetails={movieDetails} />
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};
