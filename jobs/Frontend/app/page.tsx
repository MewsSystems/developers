import SearchBar from "@/components/SearchBar";
import { Movie } from "@/interfaces/Movie";
import { MoviesResponse } from "@/interfaces/MoviesResponse";
import { constructMovieDbUrl } from "@/utils/movieDbUrl.util";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "Search",
};

const fetchData = async (query: string) => {
  const url = constructMovieDbUrl("/search/movie", query);

  const response = await fetch(url);

  if (!response.ok) {
    // Logging here will be server-side not client, so it is not exposed. Additionally, we could log to a service we may use
    console.error(
      "Could not retrieve movies. Error: ",
      response.status,
      response.statusText,
    );

    // Don't return any movies instead of erroring
    return [];
  }

  const moviesResponse = (await response.json()) as MoviesResponse;

  // Map response so we don't return any unnecessary information
  const movies: Movie[] = moviesResponse.results.map((movie) => ({
    id: movie.id,
    original_language: movie.original_language,
    original_title: movie.original_title,
    overview: movie.overview,
    popularity: movie.popularity,
    poster_path: movie.poster_path,
    release_date: movie.release_date,
    title: movie.title,
  }));

  return movies;
};

interface SearchPageProps {
  searchParams: { query: string };
}

export default async function Page({ searchParams }: SearchPageProps) {
  const movies = await fetchData(searchParams.query);
  console.log(movies); // TODO - remove

  return (
    <>
      <SearchBar />
    </>
  );
}
