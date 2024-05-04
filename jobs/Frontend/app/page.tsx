import { Metadata } from "next";
import MovieCardsContainer from "@/components/MovieCardsContainer";
import SearchBar from "@/components/SearchBar";
import { Movie } from "@/interfaces/movie";
import { MoviesDbMovie, MoviesDbSearchResponse } from "@/interfaces/MoviesDb";
import { constructMovieDbUrl } from "@/utils/movieDbUrl.util";

export const metadata: Metadata = {
  title: "Search",
};

const fetchData = async (query: string) => {
  const moviesUrl = constructMovieDbUrl("/search/movie", query);
  const moviesResponse = await fetch(moviesUrl);

  if (!moviesResponse.ok) {
    // Logging here will be server-side not client, so it is not exposed. Additionally, we could log to a service we may use
    console.error(
      "Could not retrieve movies. Error: ",
      moviesResponse.status,
      moviesResponse.statusText,
    );

    // Don't return any movies instead of erroring
    return [];
  }

  const moviesSearchResponseData =
    (await moviesResponse.json()) as MoviesDbSearchResponse<MoviesDbMovie>;

  // Map response so we don't return any unnecessary information
  const moviesData: Movie[] = moviesSearchResponseData.results.map((movie) => ({
    id: movie.id,
    originalLanguage: movie.original_language,
    originalTitle: movie.original_title,
    overview: movie.overview,
    popularity: movie.popularity,
    posterUrl: movie.poster_path,
    releaseDate: movie.release_date,
    title: movie.title,
  }));

  return moviesData;
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
      <MovieCardsContainer movies={movies} />
    </>
  );
}
