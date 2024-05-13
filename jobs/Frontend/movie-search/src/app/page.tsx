import { Metadata } from "next";
import MoviePagination from "@/components/MovieHome/MoviePagination/MoviePagination";
import { fetchMovies } from "@/data/FetchMovies";
import MovieHeader from "@/components/MovieHome/MovieHeader/MovieHeader";
import MovieList from "@/components/MovieHome/MovieList/MovieList";

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
      <MovieHeader />
      <MovieList searchterm={searchParams.searchterm} movies={movies} />
      <MoviePagination totalPages={totalPages} />
    </main>
  );
}
