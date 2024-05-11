import MovieSearchInput from "@/components/MovieSearchInput/MovieSearchInput";
import { fetchMovies } from "@/utils/FetchMovies";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "MEWS Movie Search",
  description: "Search for a movie",
};

interface HomePageProps {
  searchParams: { query: string; page: number };
}

export default async function Home({ searchParams }: HomePageProps) {
  const movies = await fetchMovies(searchParams.query, searchParams.page);

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
    </main>
  );
}
