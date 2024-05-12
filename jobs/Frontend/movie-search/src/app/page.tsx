import { Metadata } from "next";
import MoviePagination from "@/components/MoviePagination/MoviePagination";
import MovieSearchInput from "@/components/MovieSearchInput/MovieSearchInput";
import { fetchMovies } from "@/utils/FetchMovies";

export const metadata: Metadata = {
  title: "MEWS Movie Search",
  description: "Search for a movie",
};

interface HomePageProps {
  searchParams: { searchterm: string; page: number };
}

export default async function Home({ searchParams }: HomePageProps) {
  const { movies, totalPages } = await fetchMovies(
    "/search/movie",
    searchParams.searchterm,
    searchParams.page
  );

  return (
    <main>
      <h1>Movie Search</h1>
      <MovieSearchInput />
      <ul>
        {movies.map((movie) => (
          <li key={movie.id}>
            <h2>{movie.title}</h2>
            <p>{movie.overview}</p>
            <p>{movie.releaseDate}</p>
            {movie.posterPath && (
              <img
                src={`https://image.tmdb.org/t/p/w200${movie.posterPath}`}
                alt={`${movie.title} poster`}
              />
            )}
          </li>
        ))}
      </ul>
      <MoviePagination totalPages={totalPages} />
    </main>
  );
}
