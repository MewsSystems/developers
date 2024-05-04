import { Metadata } from "next";
import MovieCardsContainer from "@/components/MovieCards";
import SearchBar from "@/components/SearchBar";
import { MovieSearch } from "@/interfaces/movie";
import { MoviesDbMovie, MoviesDbSearchResponse } from "@/interfaces/MoviesDb";
import { constructMovieDbUrl } from "@/utils/movieDbUrl.util";
import Pagination from "@/components/Pagination";

export const metadata: Metadata = {
  title: "Search",
};

const fetchData = async (query: string, page?: number) => {
  const moviesUrl = constructMovieDbUrl("/search/movie", query, page);
  const moviesResponse = await fetch(moviesUrl);

  if (!moviesResponse.ok) {
    // Logging here will be server-side not client, so it is not exposed. Additionally, we could log to a service we may use
    console.error(
      "Could not retrieve movies. Error: ",
      moviesResponse.status,
      moviesResponse.statusText,
    );

    // Don't return any movies instead of erroring
    return { movies: [], totalPages: 0 };
  }

  const moviesSearchResponseData =
    (await moviesResponse.json()) as MoviesDbSearchResponse<MoviesDbMovie>;

  // Map response so we don't return any unnecessary information
  const moviesData: MovieSearch[] = moviesSearchResponseData.results.map(
    (movie) => ({
      id: movie.id,
      originalLanguage: movie.original_language,
      originalTitle: movie.original_title,
      overview: movie.overview,
      popularity: movie.popularity,
      posterUrl: movie.poster_path,
      releaseDate: movie.release_date,
      title: movie.title,
    }),
  );

  return {
    movies: moviesData,
    totalPages: moviesSearchResponseData.total_pages,
  };
};

interface SearchPageProps {
  searchParams: { query: string; page?: number };
}

export default async function Page({ searchParams }: SearchPageProps) {
  const { movies, totalPages } = await fetchData(
    searchParams.query,
    searchParams.page,
  );

  return (
    <>
      <SearchBar />
      {searchParams.query ? (
        <>
          <MovieCardsContainer movies={movies} />
          <Pagination totalPages={totalPages} />
        </>
      ) : (
        <p>Search for a movie using the search bar above</p>
      )}
    </>
  );
}
