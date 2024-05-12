import { Metadata } from "next";
import MoviePagination from "@/components/MoviePagination/MoviePagination";
import MovieSearchInput from "@/components/MovieSearchInput/MovieSearchInput";
import { fetchMovies } from "@/data/FetchMovies";
import Link from "next/link";

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
        {movies.map((movie, index) => (
          <Link key={movie.id} href={`/movie/${movie.id}?fromSearch=true`}>
            <li>
              <h2 data-testid={`movieTitle${index}`}>{movie.title}</h2>
              <p>{movie.releaseDate}</p>
              {movie.posterPath && (
                <img
                  src={`https://image.tmdb.org/t/p/w200${movie.posterPath}`}
                  alt={`${movie.title} poster`}
                />
              )}
            </li>
          </Link>
        ))}
      </ul>
      <MoviePagination totalPages={totalPages} />
    </main>
  );
}
